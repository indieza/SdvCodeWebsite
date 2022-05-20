﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Cloud;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Post.InputModels;

    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly INotificationService notificationService;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly IMapper mapper;
        private readonly AddCyclicActivity cyclicActivity;
        private readonly AddNonCyclicActivity nonCyclicActivity;

        public BlogService(
            ApplicationDbContext db,
            Cloudinary cloudinary,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            INotificationService notificationService,
            IHubContext<NotificationHub> notificationHubContext,
            IMapper mapper)
        {
            this.db = db;
            this.cloudinary = cloudinary;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.notificationService = notificationService;
            this.notificationHubContext = notificationHubContext;
            this.mapper = mapper;
            this.cyclicActivity = new AddCyclicActivity(this.db);
            this.nonCyclicActivity = new AddNonCyclicActivity(this.db);
        }

        public async Task<Tuple<string, string>> CreatePost(CreatePostIndexModel model, ApplicationUser user)
        {
            var category = await this.db.Categories
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == model.PostInputModel.CategoryName.ToUpper());
            var contentWithoutTags = Regex.Replace(model.PostInputModel.SanitizeContent, "<.*?>", string.Empty);

            var post = new Post
            {
                Title = model.PostInputModel.Title,
                CategoryId = category.Id,
                Content = model.PostInputModel.SanitizeContent,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ShortContent = contentWithoutTags.Length <= 347 ?
                    contentWithoutTags :
                    $"{contentWithoutTags.Substring(0, 347)}...",
                ApplicationUserId = user.Id,
                Likes = 0,
            };

            var imageUrl = await ApplicationCloudinary.UploadImage(
                this.cloudinary,
                model.PostInputModel.CoverImage,
                string.Format(GlobalConstants.CloudinaryPostCoverImageName, post.Id),
                GlobalConstants.PostBaseImageFolder);

            for (int i = 0; i < model.PostInputModel.PostImages.Count; i++)
            {
                var image = model.PostInputModel.PostImages.ElementAt(i);

                var postImage = new PostImage
                {
                    PostId = post.Id,
                    Name = string.Format(GlobalConstants.BlogPostImageNameTemplate, i + 1),
                };

                var postImageUrl = await ApplicationCloudinary.UploadImage(
                    this.cloudinary,
                    image,
                    string.Format(GlobalConstants.CloudinaryPostImageName, postImage.Id),
                    GlobalConstants.PostBaseImagesFolder);

                postImage.Url = postImageUrl;
                post.PostImages.Add(postImage);
            }

            if (imageUrl != null)
            {
                post.ImageUrl = imageUrl;
            }

            foreach (var tagName in model.PostInputModel.TagsNames)
            {
                var tag = await this.db.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == tagName.ToLower());
                post.PostsTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id,
                });
            }

            var adminRole = await this.roleManager.FindByNameAsync(Roles.Administrator.ToString());
            var editorRole = await this.roleManager.FindByNameAsync(Roles.Editor.ToString());

            var allAdminIds = this.db.UserRoles
                .Where(x => x.RoleId == adminRole.Id)
                .Select(x => x.UserId)
                .ToList();
            var allEditorIds = this.db.UserRoles
                .Where(x => x.RoleId == editorRole.Id)
                .Select(x => x.UserId)
                .ToList();
            var specialIds = allAdminIds.Union(allEditorIds).ToList();

            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()))
            {
                post.PostStatus = PostStatus.Approved;
                var followerIds = this.db.FollowUnfollows
                    .Where(x => x.ApplicationUserId == user.Id && !specialIds.Contains(x.FollowerId))
                    .Select(x => x.FollowerId)
                    .ToList();
                specialIds = specialIds.Union(followerIds).ToList();
                specialIds.Remove(user.Id);
            }
            else
            {
                post.PostStatus = PostStatus.Pending;
                this.db.PendingPosts.Add(new PendingPost
                {
                    ApplicationUserId = post.ApplicationUserId,
                    PostId = post.Id,
                    IsPending = true,
                });
            }

            foreach (var specialId in specialIds)
            {
                var toUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == specialId);

                string notificationId =
                    await this.notificationService.AddBlogPostNotification(toUser, user, post.ShortContent, post.Id);

                var count = await this.notificationService.GetUserNotificationsCount(toUser.UserName);
                await this.notificationHubContext
                    .Clients
                    .User(toUser.Id)
                    .SendAsync("ReceiveNotification", count, true);

                var notification = await this.notificationService.GetNotificationById(notificationId);
                await this.notificationHubContext.Clients.User(toUser.Id)
                    .SendAsync("VisualizeNotification", notification);
            }

            this.db.Posts.Add(post);
            this.db.BlockedPosts.Add(new BlockedPost
            {
                ApplicationUserId = post.ApplicationUserId,
                PostId = post.Id,
                IsBlocked = false,
            });

            //this.nonCyclicActivity.AddUserAction(user, post, UserActionType.CreatePost, user);
            await this.db.SaveChangesAsync();
            return Tuple.Create("Success", SuccessMessages.SuccessfullyCreatedPost);
        }

        public async Task<Tuple<string, string>> DeletePost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var userPost = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);

            if (post != null && userPost != null)
            {
                if (post.ImageUrl != null)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.CloudinaryPostCoverImageName, post.Id),
                        GlobalConstants.PostBaseImageFolder);
                }

                var allPostImages = this.db.PostImages
                    .Where(x => x.PostId == post.Id)
                    .ToList();

                foreach (var postImage in allPostImages)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.CloudinaryPostImageName, postImage.Id),
                        GlobalConstants.PostBaseImagesFolder);
                }

                if (user.Id == post.ApplicationUserId)
                {
                    //this.cyclicActivity.AddUserAction(user, UserActionType.DeleteOwnPost, user);
                }
                else
                {
                    //this.cyclicActivity.AddUserAction(user, UserActionType.DeletedPost, userPost);
                    //this.cyclicActivity.AddUserAction(userPost, UserActionType.DeletePost, user);
                }

                //var postActivities = this.db.UserActions.Where(x => x.PostId == post.Id);
                //var comments = this.db.Comments.Where(x => x.PostId == post.Id).ToList();
                //this.db.Comments.RemoveRange(comments);
                //this.db.UserActions.RemoveRange(postActivities);
                //this.db.PostImages.RemoveRange(allPostImages);
                //this.db.Posts.Remove(post);

                await this.db.SaveChangesAsync();
                return Tuple.Create("Success", SuccessMessages.SuccessfullyDeletePost);
            }

            return Tuple.Create("Error", SuccessMessages.SuccessfullyDeletePost);
        }

        public async Task<Tuple<string, string>> EditPost(EditPostInputModel model, ApplicationUser user)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == model.Id);
            var contentWithoutTags = Regex.Replace(model.SanitizeContent, "<.*?>", string.Empty);

            if (post != null)
            {
                var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
                var postUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = category;
                post.Title = model.Title;
                post.UpdatedOn = DateTime.UtcNow;
                post.Content = model.SanitizeContent;
                post.ShortContent = contentWithoutTags.Length <= 347 ?
                    contentWithoutTags :
                    $"{contentWithoutTags.Substring(0, 347)}...";

                var imageUrl = await ApplicationCloudinary.UploadImage(
                    this.cloudinary,
                    model.CoverImage,
                    string.Format(
                        GlobalConstants.CloudinaryPostCoverImageName,
                        post.Id),
                    GlobalConstants.PostBaseImageFolder);

                if (imageUrl != null)
                {
                    post.ImageUrl = imageUrl;
                }

                if (model.TagsNames.Count > 0)
                {
                    List<PostTag> oldTagsIds = this.db.PostsTags.Where(x => x.PostId == model.Id).ToList();
                    this.db.PostsTags.RemoveRange(oldTagsIds);

                    List<PostTag> postTags = new List<PostTag>();
                    foreach (var tagName in model.TagsNames)
                    {
                        var tag = await this.db.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == tagName.ToLower());
                        postTags.Add(new PostTag
                        {
                            PostId = post.Id,
                            TagId = tag.Id,
                        });
                    }

                    post.PostsTags = postTags;
                }

                if (user.Id == postUser.Id)
                {
                    //this.nonCyclicActivity.AddUserAction(user, post, UserActionType.EditOwnPost, user);
                }
                else
                {
                    //this.nonCyclicActivity.AddUserAction(user, post, UserActionType.EditPost, postUser);
                    //this.nonCyclicActivity.AddUserAction(postUser, post, UserActionType.EditedPost, user);
                }

                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return Tuple.Create("Success", SuccessMessages.SuccessfullyEditedPost);
            }

            return Tuple.Create("Error", ErrorMessages.InvalidInputModel);
        }

        /// <summary>
        /// This function get all Categories Names from the Database.
        /// </summary>
        /// <returns>Returns a Collection of Strings including all Categories Names.</returns>
        public async Task<ICollection<string>> ExtractAllCategoryNames()
        {
            return await this.db.Categories.Select(x => x.Name).OrderBy(x => x).ToListAsync();
        }

        /// <summary>
        /// This function get all Tag Names from the Database.
        /// </summary>
        /// <returns>Returns a Collection of Strings including all Tags Names.</returns>
        public async Task<ICollection<string>> ExtractAllTagNames()
        {
            return await this.db.Tags.Select(x => x.Name).OrderBy(x => x).ToListAsync();
        }

        /// <summary>
        /// This function will return a target post for editing by its ID from the database.
        /// </summary>
        /// <param name="id">ID of the current accessed Blog Post.</param>
        /// <returns>Returns a input model.</returns>
        public async Task<EditPostInputModel> ExtractPost(string id)
        {
            var post = await this.db.Posts
                .Include(x => x.Category)
                .Include(x => x.PostsTags)
                .ThenInclude(x => x.Tag)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            var model = this.mapper.Map<EditPostInputModel>(post);
            return model;
        }

        public async Task<ICollection<BlogPostCardViewModel>> ExtraxtAllPosts(ApplicationUser user, string search)
        {
            var posts = new List<Post>();
            Expression<Func<Post, bool>> filterFunction;

            if (user != null &&
                (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                filterFunction = x => x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending ||
                    x.PostStatus == PostStatus.Approved;
            }
            else
            {
                if (user != null)
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == user.Id;
                }
                else
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved;
                }
            }

            if (search == null)
            {
                posts = this.db.Posts
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Category)
                    .Include(x => x.Comments)
                    .Include(x => x.FavouritePosts)
                    .Include(x => x.PostLikes)
                    .Where(filterFunction)
                    .OrderByDescending(x => x.UpdatedOn)
                    .AsSplitQuery()
                    .ToList();
            }
            else
            {
                posts = this.db.Posts
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Category)
                    .Include(x => x.Comments)
                    .Include(x => x.FavouritePosts)
                    .Include(x => x.PostLikes)
                    .Where(x => EF.Functions.FreeText(x.Title, search) ||
                    EF.Functions.FreeText(x.ShortContent, search) ||
                    EF.Functions.FreeText(x.Content, search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .AsSplitQuery()
                    .ToList();
            }

            var postsModel = this.mapper.Map<List<BlogPostCardViewModel>>(posts);
            return postsModel;
        }

        /// <summary>
        /// This function will check is a target post exist by its ID in the database.
        /// </summary>
        /// <param name="id">ID of the current accessed Blog Post.</param>
        /// <returns>Returns a boolean value, is the Blog Post exist.</returns>
        public async Task<bool> IsPostExist(string id)
        {
            return await this.db.Posts.AnyAsync(x => x.Id == id);
        }
    }
}