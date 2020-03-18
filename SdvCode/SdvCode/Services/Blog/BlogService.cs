// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.CloudServices;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;

    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public BlogService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<bool> CreatePost(CreatePostIndexModel model, ApplicationUser user)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Name == model.PostInputModel.CategoryName);

            var post = new Post
            {
                Title = model.PostInputModel.Title,
                Category = category,
                Content = model.PostInputModel.SanitizeContent,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ShortContent = model.PostInputModel.SanitizeContent.Length < 347 ?
                    model.PostInputModel.Content :
                    $"{model.PostInputModel.SanitizeContent.Substring(0, 347)}...",
                ApplicationUser = user,
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

            this.db.Posts.Add(post);
            this.db.UserActions.Add(new UserAction
            {
                Action = UserActionsType.CreatePost,
                ActionDate = DateTime.UtcNow,
                ApplicationUserId = user.Id,
                PostId = post.Id,
                PersonProfileImageUrl = user.ImageUrl,
                PersonUsername = user.UserName,
                ProfileImageUrl = user.ImageUrl,
            });
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var userPost = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);

            if (post != null && userPost != null)
            {
                if (post.ImageUrl != null)
                {
                    ApplicationCloudinary
                        .DeleteImage(this.cloudinary, string.Format(GlobalConstants.CloudinaryPostCoverImageName, post.Id));
                }

                if (user.Id == post.ApplicationUserId)
                {
                    if (this.db.UserActions
                        .Any(x => x.Action == UserActionsType.DeleteOwnPost &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName))
                    {
                        this.db.UserActions
                            .FirstOrDefault(x => x.Action == UserActionsType.DeleteOwnPost &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName).ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.DeleteOwnPost,
                            ActionDate = DateTime.UtcNow,
                            ApplicationUserId = user.Id,
                            PersonUsername = user.UserName,
                            PersonProfileImageUrl = user.ImageUrl,
                        });
                    }
                }
                else
                {
                    if (this.db.UserActions
                        .Any(x => x.Action == UserActionsType.DeletedPost &&
                        x.ApplicationUserId == userPost.Id &&
                        x.PersonUsername == userPost.UserName &&
                        x.FollowerUsername == user.UserName))
                    {
                        this.db.UserActions
                            .FirstOrDefault(x => x.Action == UserActionsType.DeletedPost &&
                            x.ApplicationUserId == userPost.Id &&
                            x.PersonUsername == userPost.UserName &&
                            x.FollowerUsername == user.UserName).ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.DeletedPost,
                            ActionDate = DateTime.UtcNow,
                            ApplicationUserId = userPost.Id,
                            PersonUsername = userPost.UserName,
                            FollowerUsername = user.UserName,
                            FollowerProfileImageUrl = user.ImageUrl,
                        });
                    }

                    if (this.db.UserActions
                        .Any(x => x.Action == UserActionsType.DeletePost &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName &&
                        x.FollowerUsername == userPost.UserName))
                    {
                        this.db.UserActions
                            .FirstOrDefault(x => x.Action == UserActionsType.DeletePost &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName &&
                            x.FollowerUsername == userPost.UserName).ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.DeletePost,
                            ActionDate = DateTime.UtcNow,
                            ApplicationUserId = user.Id,
                            PersonUsername = user.UserName,
                            FollowerUsername = userPost.UserName,
                            FollowerProfileImageUrl = userPost.ImageUrl,
                        });
                    }
                }

                var postsLikes = this.db.PostsLikes.Where(x => x.PostId == post.Id);
                var postActivities = this.db.UserActions.Where(x => x.PostId == post.Id);
                this.db.UserActions.RemoveRange(postActivities);
                this.db.PostsLikes.RemoveRange(postsLikes);
                this.db.Posts.Remove(post);

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> EditPost(EditPostInputModel model, ApplicationUser user)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (post != null)
            {
                var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
                var postUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = category;
                post.Title = model.Title;
                post.UpdatedOn = DateTime.UtcNow;
                post.Content = model.SanitizeContent;
                post.ShortContent = model.SanitizeContent.Length < 347 ?
                    model.SanitizeContent :
                    $"{model.SanitizeContent.Substring(0, 347)}...";

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
                    this.db.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.EditOwnPost,
                        ActionDate = DateTime.UtcNow,
                        ApplicationUserId = user.Id,
                        PersonUsername = user.UserName,
                        PersonProfileImageUrl = user.ImageUrl,
                        PostId = post.Id,
                        PostTitle = post.Title,
                        PostContent = post.Content.Length < 347 ?
                        post.Content :
                        $"{post.Content.Substring(0, 347)}...",
                    });
                }
                else
                {
                    this.db.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.EditPost,
                        ActionDate = DateTime.UtcNow,
                        ApplicationUserId = user.Id,
                        PersonUsername = user.UserName,
                        FollowerUsername = postUser.UserName,
                        FollowerProfileImageUrl = postUser.ImageUrl,
                        PostId = post.Id,
                        PostTitle = post.Title,
                        PostContent = post.Content.Length < 347 ?
                        post.Content :
                        $"{post.Content.Substring(0, 347)}...",
                    });

                    this.db.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.EditedPost,
                        ActionDate = DateTime.UtcNow,
                        ApplicationUserId = postUser.Id,
                        PersonUsername = postUser.UserName,
                        FollowerUsername = user.UserName,
                        FollowerProfileImageUrl = user.ImageUrl,
                        PostId = post.Id,
                        PostTitle = post.Title,
                        PostContent = post.Content.Length < 347 ?
                        post.Content :
                        $"{post.Content.Substring(0, 347)}...",
                    });
                }

                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ICollection<string>> ExtractAllCategoryNames()
        {
            return await this.db.Categories.Select(x => x.Name).ToListAsync();
        }

        public async Task<ICollection<string>> ExtractAllTagNames()
        {
            return await this.db.Tags.Select(x => x.Name).ToListAsync();
        }

        public async Task<EditPostInputModel> ExtractPost(string id, ApplicationUser user)
        {
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

        public async Task<ICollection<Post>> ExtraxtAllPosts(ApplicationUser user)
        {
            var posts = await this.db.Posts.OrderByDescending(x => x.CreatedOn).ToListAsync();

            foreach (var post in posts)
            {
                post.Category = this.db.Categories.FirstOrDefault(x => x.Id == post.CategoryId);
                post.ApplicationUser = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id && x.IsLiked == true);
                if (user != null)
                {
                    post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == user.Id && x.IsLiked == true);
                }
                else
                {
                    post.IsLiked = false;
                }
            }

            return posts;
        }
    }
}