// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditorPostService : IEditorPostService
    {
        private readonly ApplicationDbContext db;

        public EditorPostService(ApplicationDbContext db)
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