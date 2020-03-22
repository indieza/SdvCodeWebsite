// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Cloud;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Post.InputModels;
    using SdvCode.ViewModels.Post.ViewModels;

    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly GlobalPostsExtractor postExtractor;
        private readonly AddCyclicActivity cyclicActivity;
        private readonly AddNonCyclicActivity nonCyclicActivity;

        public BlogService(
            ApplicationDbContext db,
            Cloudinary cloudinary,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.cloudinary = cloudinary;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.postExtractor = new GlobalPostsExtractor(this.db);
            this.cyclicActivity = new AddCyclicActivity(this.db);
            this.nonCyclicActivity = new AddNonCyclicActivity(this.db);
        }

        public async Task<bool> CreatePost(CreatePostIndexModel model, HttpContext httpContext)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Name == model.PostInputModel.CategoryName);
            var user = await this.userManager.GetUserAsync(httpContext.User);
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
                string.Format(
                    GlobalConstants.CloudinaryPostCoverImageName,
                    post.Id));

            if (imageUrl != null)
            {
                post.ImageUrl = imageUrl;
            }

            foreach (var tagName in model.PostInputModel.TagsNames)
            {
                var tag = this.db.Tags.FirstOrDefault(x => x.Name.ToLower() == tagName.ToLower());
                post.PostsTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id,
                });
            }

            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()))
            {
                post.PostStatus = PostStatus.Approved;
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

            this.db.Posts.Add(post);
            this.db.BlockedPosts.Add(new Models.Blog.BlockedPost
            {
                ApplicationUserId = post.ApplicationUserId,
                PostId = post.Id,
                IsBlocked = false,
            });

            this.nonCyclicActivity.AddUserAction(user, post, UserActionsType.CreatePost, user);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(string id, HttpContext httpContext)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var userPost = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
            var user = await this.userManager.GetUserAsync(httpContext.User);

            if (post != null && userPost != null)
            {
                if (post.ImageUrl != null)
                {
                    ApplicationCloudinary
                        .DeleteImage(this.cloudinary, string.Format(GlobalConstants.CloudinaryPostCoverImageName, post.Id));
                }

                if (user.Id == post.ApplicationUserId)
                {
                    this.cyclicActivity.AddUserAction(user, UserActionsType.DeleteOwnPost, user);
                }
                else
                {
                    this.cyclicActivity.AddUserAction(user, UserActionsType.DeletedPost, userPost);
                    this.cyclicActivity.AddUserAction(userPost, UserActionsType.DeletePost, user);
                }

                var postActivities = this.db.UserActions.Where(x => x.PostId == post.Id);
                this.db.UserActions.RemoveRange(postActivities);
                this.db.Posts.Remove(post);

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> EditPost(EditPostInputModel model, HttpContext httpContext)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
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
                        post.Id));

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
                    this.nonCyclicActivity.AddUserAction(user, post, UserActionsType.EditOwnPost, user);
                }
                else
                {
                    this.nonCyclicActivity.AddUserAction(user, post, UserActionsType.EditPost, postUser);
                    this.nonCyclicActivity.AddUserAction(postUser, post, UserActionsType.EditedPost, user);
                }

                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ICollection<string>> ExtractAllCategoryNames()
        {
            return await this.db.Categories.Select(x => x.Name).OrderBy(x => x).ToListAsync();
        }

        public async Task<ICollection<string>> ExtractAllTagNames()
        {
            return await this.db.Tags.Select(x => x.Name).OrderBy(x => x).ToListAsync();
        }

        public async Task<EditPostInputModel> ExtractPost(string id, HttpContext httpContext)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == id);
            post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
            var postTagsNames = new List<string>();

            foreach (var tag in post.PostsTags)
            {
                postTagsNames.Add(this.db.Tags.FirstOrDefault(x => x.Id == tag.TagId).Name);
            }

            return new EditPostInputModel
            {
                Id = post.Id,
                Title = post.Title,
                CategoryName = post.Category.Name,
                Content = post.Content,
                TagsNames = postTagsNames,
                Tags = postTagsNames,
            };
        }

        public async Task<ICollection<PostViewModel>> ExtraxtAllPosts(HttpContext httpContext)
        {
            var posts = await this.db.Posts.OrderByDescending(x => x.CreatedOn).ToListAsync();
            var user = await this.userManager.GetUserAsync(httpContext.User);
            List<PostViewModel> postsModel = await this.postExtractor.ExtractPosts(user, posts);
            return postsModel;
        }
    }
}