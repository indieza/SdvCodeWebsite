// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Comment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Blog;

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext db;

        public CommentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(string postId, string userId, string content, string parentId)
        {
            var comment = new Comment
            {
                Content = content,
                ParentCommentId = parentId,
                PostId = postId,
                ApplicationUserId = userId,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();
            return true;
        }

        public bool IsInPostId(string commentId, string postId)
        {
            var commentPostId = this.db.Comments
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId).FirstOrDefault();

            return commentPostId == postId;
        }
    }
}