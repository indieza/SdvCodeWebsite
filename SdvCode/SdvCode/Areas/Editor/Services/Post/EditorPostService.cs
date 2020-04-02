// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Services;

    public class EditorPostService : UserValidationService, IEditorPostService
    {
        private readonly ApplicationDbContext db;
        private readonly List<UserActionsType> actionsForBann = new List<UserActionsType>()
        {
            UserActionsType.LikedPost,
            UserActionsType.LikePost,
            UserActionsType.LikeOwnPost,
            UserActionsType.EditedPost,
            UserActionsType.EditPost,
            UserActionsType.EditOwnPost,
            UserActionsType.CreatePost,
            UserActionsType.UnlikedPost,
            UserActionsType.UnlikePost,
            UserActionsType.UnlikeOwnPost,
        };

        public EditorPostService(ApplicationDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<bool> ApprovePost(string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var targetApprovedEntity = this.db.PendingPosts.FirstOrDefault(x => x.PostId == post.Id && x.IsPending == true);

            if (post != null && targetApprovedEntity != null)
            {
                post.PostStatus = PostStatus.Approved;
                targetApprovedEntity.IsPending = false;
                this.db.PendingPosts.Update(targetApprovedEntity);
                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> BannPost(string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var targetApprovedEntity = this.db.BlockedPosts.FirstOrDefault(x => x.PostId == post.Id && x.IsBlocked == false);

            if (post != null && targetApprovedEntity != null)
            {
                post.PostStatus = PostStatus.Banned;
                targetApprovedEntity.IsBlocked = true;
                var favorites = this.db.FavouritePosts.Where(x => x.PostId == id);

                foreach (var favor in favorites)
                {
                    favor.IsFavourite = false;
                }

                var likes = this.db.PostsLikes.Where(x => x.PostId == id && x.IsLiked == true);

                foreach (var like in likes)
                {
                    like.IsLiked = false;
                    post.Likes--;
                }

                var actions = this.db.UserActions
                    .Where(x => x.PostId == id && this.actionsForBann.Contains(x.Action));

                this.db.FavouritePosts.UpdateRange(favorites);
                this.db.PostsLikes.UpdateRange(likes);
                this.db.UserActions.RemoveRange(actions);
                this.db.BlockedPosts.Update(targetApprovedEntity);
                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UnbannPost(string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var targetApprovedEntity = this.db.BlockedPosts.FirstOrDefault(x => x.PostId == post.Id && x.IsBlocked == true);

            if (post != null && targetApprovedEntity != null)
            {
                post.PostStatus = PostStatus.Approved;
                targetApprovedEntity.IsBlocked = false;
                this.db.BlockedPosts.Update(targetApprovedEntity);
                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}