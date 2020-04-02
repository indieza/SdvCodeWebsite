// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Comment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<Tuple<string, string>> Create(string postId, ApplicationUser user, string content, string parentId)
        {
            var comment = new Comment
            {
                Content = content,
                ParentCommentId = parentId,
                PostId = postId,
                ApplicationUserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()))
            {
                comment.CommentStatus = CommentStatus.Approved;
            }
            else
            {
                comment.CommentStatus = CommentStatus.Pending;
            }

            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();
            return Tuple.Create("Success", SuccessMessages.SuccessfullyAddedPostComment);
        }

        public bool IsInPostId(string commentId, string postId)
        {
            var commentPostId = this.db.Comments
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId).FirstOrDefault();

            return commentPostId == postId;
        }

        public async Task<Tuple<string, string>> DeleteCommentById(string commentId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(m => m.Id == commentId);
            if (comment != null)
            {
                if (comment != null)
                {
                    await this.RemoveChildren(comment.Id);
                    this.db.Comments.Remove(comment);
                }

                await this.db.SaveChangesAsync();
                return Tuple.Create("Success", SuccessMessages.SuccessfullyDeletePostComment);
            }

            return Tuple.Create("Error", ErrorMessages.InvalidInputModel);
        }

        public async Task<bool> IsParentCommentApproved(string parentId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == parentId);
            return comment.CommentStatus == CommentStatus.Approved ? true : false;
        }

        public async Task<Post> ExtractCurrentPost(string postId)
        {
            return await this.db.Posts.FirstOrDefaultAsync(x => x.Id == postId);
        }

        private async Task RemoveChildren(string i)
        {
            var children = await this.db.Comments.Where(c => c.ParentCommentId == i).ToListAsync();
            foreach (var child in children)
            {
                await this.RemoveChildren(child.Id);
                this.db.Comments.Remove(child);
            }
        }
    }
}