// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;

        public PostService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public PostViewModel ExtractCurrentPost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            PostViewModel model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id && x.IsLiked == true),
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                UpdatedOn = post.UpdatedOn,
                Comments = post.Comments,
                ImageUrl = post.ImageUrl,
                IsLiked = this.db.PostsLikes.Any(x => x.PostId == id && x.UserId == user.Id && x.IsLiked == true),
            };

            model.ApplicationUser = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
            model.Category = this.db.Categories.FirstOrDefault(x => x.Id == post.CategoryId);

            foreach (var tag in post.PostsTags)
            {
                var curretnTag = this.db.Tags.FirstOrDefault(x => x.Id == tag.TagId);
                model.Tags.Add(new Tag
                {
                    Id = curretnTag.Id,
                    CreatedOn = curretnTag.CreatedOn,
                    Name = curretnTag.Name,
                });
            }

            return model;
        }

        public async Task<bool> LikePost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);

            if (post != null)
            {
                post.Likes++;
                this.db.Posts.Update(post);
                var targetLike = this.db.PostsLikes.FirstOrDefault(x => x.PostId == id && x.UserId == user.Id);

                if (targetLike != null && targetLike.IsLiked == false)
                {
                    targetLike.IsLiked = true;
                }
                else if (targetLike != null && targetLike.IsLiked == true)
                {
                    targetLike.IsLiked = false;
                }
                else
                {
                    this.db.PostsLikes.Add(new PostLike
                    {
                        UserId = user.Id,
                        PostId = id,
                        IsLiked = true,
                    });
                }

                if (this.db.UserActions.Any(x => x.PostId == id && x.ApplicationUserId == user.Id && x.Action == UserActionsType.LikePost))
                {
                    var targetAction = this.db.UserActions
                        .FirstOrDefault(x => x.PostId == id && x.ApplicationUserId == user.Id && x.Action == UserActionsType.LikePost);
                    targetAction.ActionDate = DateTime.UtcNow;
                }
                else
                {
                    this.db.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.LikePost,
                        ActionDate = DateTime.UtcNow,
                        PostId = id,
                        PersonProfileImageUrl = user.ImageUrl,
                        PersonUsername = user.UserName,
                        ApplicationUserId = user.Id,
                        ProfileImageUrl = user.ImageUrl,
                    });
                }

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UnlikePost(string id, ApplicationUser user)
        {
            var targetPost = this.db.Posts.FirstOrDefault(x => x.Id == id);

            var targetPostLike = this.db.PostsLikes.FirstOrDefault(x => x.PostId == id && x.UserId == user.Id);
            if (targetPostLike != null && targetPostLike.IsLiked == true)
            {
                targetPostLike.IsLiked = false;
                targetPost.Likes--;
                if (this.db.UserActions.Any(x => x.PostId == id && x.ApplicationUserId == user.Id && x.Action == UserActionsType.UnlikePost))
                {
                    var targetAction = this.db.UserActions
                        .FirstOrDefault(x => x.PostId == id && x.ApplicationUserId == user.Id && x.Action == UserActionsType.LikePost);
                    targetAction.ActionDate = DateTime.UtcNow;
                }
                else
                {
                    this.db.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.UnlikePost,
                        ActionDate = DateTime.UtcNow,
                        ApplicationUser = user,
                        PersonProfileImageUrl = user.ImageUrl,
                        PersonUsername = user.UserName,
                        PostId = id,
                        ProfileImageUrl = user.ImageUrl,
                    });
                }
                await this.db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}