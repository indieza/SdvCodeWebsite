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
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;
        private readonly AddCyclicActivity cyclicActivity;

        public PostService(ApplicationDbContext db)
        {
            this.db = db;
            this.cyclicActivity = new AddCyclicActivity(this.db);
        }

        public async Task<bool> AddToFavorite(ApplicationUser user, string id)
        {
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

        public async Task<PostViewModel> ExtractCurrentPost(string id, ApplicationUser user)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == id);
            post.Comments = this.db.Comments.Where(x => x.PostId == post.Id).OrderBy(x => x.CreatedOn).ToList();
            foreach (var comment in post.Comments)
            {
                comment.ApplicationUser = this.db.Users.FirstOrDefault(x => x.Id == comment.ApplicationUserId);
            }

            post.PostsTags = this.db.PostsTags.Where(x => x.PostId == post.Id).ToList();
            PostViewModel model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Likes = post.Likes,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                UpdatedOn = post.UpdatedOn,
                Comments = post.Comments,
                ImageUrl = post.ImageUrl,
                IsLiked = this.db.PostsLikes.Any(x => x.PostId == id && x.UserId == user.Id && x.IsLiked == true),
                IsAuthor = post.ApplicationUserId == user.Id ? true : false,
                IsFavourite = this.db.FavouritePosts.Any(x => x.ApplicationUserId == user.Id && x.PostId == post.Id && x.IsFavourite == true),
                PostStatus = post.PostStatus,
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

        public async Task<bool> LikePost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
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
                    this.cyclicActivity.AddLikeUnlikeActivity(user, post, UserActionsType.LikeOwnPost, user);
                }
                else
                {
                    this.cyclicActivity.AddLikeUnlikeActivity(post.ApplicationUser, post, UserActionsType.LikedPost, user);
                    this.cyclicActivity.AddLikeUnlikeActivity(user, post, UserActionsType.LikePost, post.ApplicationUser);
                }

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveFromFavorite(ApplicationUser user, string id)
        {
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

        public async Task<bool> UnlikePost(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            post.ApplicationUser = this.db.Users.Find(post.ApplicationUserId);

            var targetPostsLikes = this.db.PostsLikes.FirstOrDefault(x => x.PostId == id && x.UserId == user.Id);
            if (targetPostsLikes != null && targetPostsLikes.IsLiked == true)
            {
                targetPostsLikes.IsLiked = false;
                post.Likes--;

                if (post.ApplicationUserId == user.Id)
                {
                    this.cyclicActivity.AddLikeUnlikeActivity(user, post, UserActionsType.UnlikeOwnPost, user);
                }
                else
                {
                    this.cyclicActivity.AddLikeUnlikeActivity(post.ApplicationUser, post, UserActionsType.UnlikedPost, user);
                    this.cyclicActivity.AddLikeUnlikeActivity(user, post, UserActionsType.UnlikePost, post.ApplicationUser);
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