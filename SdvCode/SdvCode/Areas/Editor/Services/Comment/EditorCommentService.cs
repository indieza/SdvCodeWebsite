// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Comment
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Enums;

    public class EditorCommentService : IEditorCommentService
    {
        private readonly ApplicationDbContext db;

        public EditorCommentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> ApprovedCommentById(string commentId)
        {
            var targetComment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (targetComment != null)
            {
                targetComment.CommentStatus = CommentStatus.Approved;
                this.db.Comments.Update(targetComment);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}