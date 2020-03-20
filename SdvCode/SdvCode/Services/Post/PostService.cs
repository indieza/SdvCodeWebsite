// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public PostService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<bool> AddToFavorite(HttpContext httpContext, string id)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);

            if (user != null && id != null)
            {
                if (this.db.FavouritePosts.Any(x => x.PostId == id && x.ApplicationUserId == user.Id))
                {
                    this.db.FavouritePosts.FirstOrDefault(x => x.PostId == id && x.ApplicationUserId == user.Id).IsFavourite = true;
                }
                else
                {
                    this.db.FavouritePosts.Add(new FavouritePost
                    {
                        ApplicationUserId = user.Id,
                        PostId = id,
                        IsFavourite = true,
                    });
                }

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<PostViewModel> ExtractCurrentPost(string id, HttpContext httpContext)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var user = await this.userManager.GetUserAsync(httpContext.User);
            post.PostsTags = this.db.PostsTags.Where(x => x.PostId == post.Id).ToList();
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
                IsAuthor = post.ApplicationUserId == user.Id ? true : false,
                IsFavourite = this.db.FavouritePosts.Any(x => x.ApplicationUserId == user.Id && x.PostId == post.Id && x.IsFavourite == true),
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

            var usersIds = this.db.PostsLikes.Where(x => x.PostId == post.Id && x.IsLiked == true).Select(x => x.UserId).ToList();
            foreach (var userId in usersIds)
            {
                model.Likers.Add(this.db.Users.FirstOrDefault(x => x.Id == userId));
            }

            return model;
        }

        public async Task<bool> LikePost(string id, HttpContext httpContext)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var user = await this.userManager.GetUserAsync(httpContext.User);
            post.ApplicationUser = this.db.Users.Find(post.ApplicationUserId);

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

                if (post.ApplicationUserId == user.Id)
                {
                    if (this.db.UserActions
                        .Any(x => x.PostId == id &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName &&
                        x.Action == UserActionsType.LikeOwnPost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(x => x.PostId == id &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName &&
                            x.Action == UserActionsType.LikeOwnPost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.LikeOwnPost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            PersonUsername = user.UserName,
                            ApplicationUserId = user.Id,
                            ProfileImageUrl = user.ImageUrl,
                            PostTitle = post.Title,
                            PostContent = post.ShortContent,
                        });
                    }
                }
                else
                {
                    if (this.db.UserActions
                        .Any(x => x.PostId == id &&
                        x.ApplicationUserId == post.ApplicationUser.Id &&
                        x.PersonUsername == post.ApplicationUser.UserName &&
                        x.FollowerUsername == user.UserName &&
                        x.Action == UserActionsType.LikedPost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(x => x.PostId == id &&
                            x.ApplicationUserId == post.ApplicationUser.Id &&
                            x.PersonUsername == post.ApplicationUser.UserName &&
                            x.FollowerUsername == user.UserName &&
                            x.Action == UserActionsType.LikedPost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.LikedPost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            PersonUsername = post.ApplicationUser.UserName,
                            ApplicationUserId = post.ApplicationUserId,
                            ApplicationUser = post.ApplicationUser,
                            FollowerUsername = user.UserName,
                            ProfileImageUrl = user.ImageUrl,
                            PostTitle = post.Title,
                            PostContent = post.ShortContent,
                        });
                    }

                    if (this.db.UserActions
                        .Any(x => x.PostId == id &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName &&
                        x.FollowerUsername == post.ApplicationUser.UserName &&
                        x.Action == UserActionsType.LikePost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(x => x.PostId == id &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName &&
                            x.FollowerUsername == post.ApplicationUser.UserName &&
                            x.Action == UserActionsType.LikePost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.LikePost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            PersonUsername = user.UserName,
                            ApplicationUserId = user.Id,
                            FollowerUsername = post.ApplicationUser.UserName,
                            ProfileImageUrl = post.ApplicationUser.ImageUrl,
                            PostTitle = post.Title,
                            PostContent = post.ShortContent,
                        });
                    }
                }

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveFromFavorite(HttpContext httpContext, string id)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);

            if (user != null && id != null)
            {
                if (this.db.FavouritePosts.Any(x => x.PostId == id && x.ApplicationUserId == user.Id))
                {
                    this.db.FavouritePosts.FirstOrDefault(x => x.PostId == id && x.ApplicationUserId == user.Id).IsFavourite = false;
                }
                else
                {
                    return false;
                }

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UnlikePost(string id, HttpContext httpContext)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var user = await this.userManager.GetUserAsync(httpContext.User);
            post.ApplicationUser = this.db.Users.Find(post.ApplicationUserId);

            var targetPostsLikes = this.db.PostsLikes.FirstOrDefault(x => x.PostId == id && x.UserId == user.Id);
            if (targetPostsLikes != null && targetPostsLikes.IsLiked == true)
            {
                targetPostsLikes.IsLiked = false;
                post.Likes--;

                if (post.ApplicationUserId == user.Id)
                {
                    if (this.db.UserActions
                        .Any(x => x.PostId == id &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName &&
                        x.Action == UserActionsType.UnlikeOwnPost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(x => x.PostId == id &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName &&
                            x.Action == UserActionsType.UnlikeOwnPost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.UnlikeOwnPost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            ProfileImageUrl = user.ImageUrl,
                            PersonUsername = user.UserName,
                            ApplicationUserId = user.Id,
                        });
                    }
                }
                else
                {
                    if (this.db.UserActions
                        .Any(x => x.PostId == id &&
                        x.ApplicationUserId == post.ApplicationUser.Id &&
                        x.PersonUsername == post.ApplicationUser.UserName &&
                        x.FollowerUsername == user.UserName &&
                        x.Action == UserActionsType.UnlikedPost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(x => x.PostId == id &&
                            x.ApplicationUserId == post.ApplicationUser.Id &&
                            x.PersonUsername == post.ApplicationUser.UserName &&
                            x.FollowerUsername == user.UserName &&
                            x.Action == UserActionsType.UnlikedPost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.UnlikedPost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            PersonUsername = post.ApplicationUser.UserName,
                            ApplicationUserId = post.ApplicationUserId,
                            ApplicationUser = post.ApplicationUser,
                            FollowerUsername = user.UserName,
                            ProfileImageUrl = user.ImageUrl,
                        });
                    }

                    if (this.db.UserActions.Any(
                        x => x.PostId == id &&
                        x.ApplicationUserId == user.Id &&
                        x.PersonUsername == user.UserName &&
                        x.FollowerUsername == post.ApplicationUser.UserName &&
                        x.Action == UserActionsType.UnlikePost))
                    {
                        var targetAction = this.db.UserActions
                            .FirstOrDefault(
                            x => x.PostId == id &&
                            x.ApplicationUserId == user.Id &&
                            x.PersonUsername == user.UserName &&
                            x.FollowerUsername == post.ApplicationUser.UserName &&
                            x.Action == UserActionsType.UnlikePost);
                        targetAction.ActionDate = DateTime.UtcNow;
                    }
                    else
                    {
                        this.db.UserActions.Add(new UserAction
                        {
                            Action = UserActionsType.UnlikePost,
                            ActionDate = DateTime.UtcNow,
                            PostId = id,
                            PersonUsername = user.UserName,
                            ApplicationUserId = user.Id,
                            FollowerUsername = post.ApplicationUser.UserName,
                            ProfileImageUrl = post.ApplicationUser.ImageUrl,
                        });
                    }
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