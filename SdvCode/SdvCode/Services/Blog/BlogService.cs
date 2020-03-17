// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
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
                Content = model.PostInputModel.Content,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ShortContent = $"{model.PostInputModel.Content.Substring(0, 347)}...",
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
                this.db.PostsLikes.RemoveRange(postsLikes);
                this.db.Posts.Remove(post);

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public ICollection<string> ExtractAllCategoryNames()
        {
            return this.db.Categories.Select(x => x.Name).ToList();
        }

        public ICollection<string> ExtractAllTagNames()
        {
            return this.db.Tags.Select(x => x.Name).ToList();
        }

        public ICollection<Post> ExtraxtAllPosts(ApplicationUser user)
        {
            var posts = this.db.Posts.OrderByDescending(x => x.CreatedOn).ToList();

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