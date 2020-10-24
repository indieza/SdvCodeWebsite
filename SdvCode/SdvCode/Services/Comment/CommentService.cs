// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Comment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Comment.ViewModels;

    public class CommentService : UserValidationService, ICommentService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly INotificationService notificationService;

        public CommentService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService)
            : base(userManager, db)
        {
            this.db = db;
            this.userManager = userManager;
            this.notificationHubContext = notificationHubContext;
            this.notificationService = notificationService;
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

            var adminRole =
                await this.db.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Administrator.ToString());
            var editorRole =
                await this.db.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Editor.ToString());

            var allAdminIds = this.db.UserRoles
                .Where(x => x.RoleId == adminRole.Id)
                .Select(x => x.UserId)
                .ToList();
            var allEditorIds = this.db.UserRoles
                .Where(x => x.RoleId == editorRole.Id)
                .Select(x => x.UserId)
                .ToList();
            var specialIds = allAdminIds.Union(allEditorIds).ToList();

            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()))
            {
                var targetUserId = string.Empty;
                if (parentId == null)
                {
                    targetUserId = await this.db.Posts
                        .Where(x => x.Id == postId)
                        .Select(x => x.ApplicationUserId)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    targetUserId = await this.db.Comments
                        .Where(x => x.Id == parentId)
                        .Select(x => x.ApplicationUserId)
                        .FirstOrDefaultAsync();
                }

                if (!specialIds.Contains(targetUserId))
                {
                    specialIds.Add(targetUserId);
                }

                comment.CommentStatus = CommentStatus.Approved;
                specialIds.Remove(user.Id);
            }
            else
            {
                comment.CommentStatus = CommentStatus.Pending;
            }

            foreach (var specialId in specialIds)
            {
                ApplicationUser toUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == specialId);
                var notificationId = string.Empty;

                if (toUser.Id != user.Id)
                {
                    if (parentId == null)
                    {
                        notificationId =
                           await this.notificationService.AddCommentPostNotification(toUser, user, content, postId);
                    }
                    else
                    {
                        notificationId = await this.notificationService
                           .AddCommentReplyNotification(toUser, user, content, postId);
                    }

                    var count = await this.notificationService.GetUserNotificationsCount(toUser.UserName);
                    await this.notificationHubContext
                        .Clients
                        .User(toUser.Id)
                        .SendAsync("ReceiveNotification", count, true);

                    var notification = await this.notificationService.GetNotificationById(notificationId);
                    await this.notificationHubContext.Clients.User(toUser.Id)
                        .SendAsync("VisualizeNotification", notification);
                }
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
                await this.RemoveChildren(comment.Id);
                this.db.Comments.Remove(comment);
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

        public async Task<bool> IsCommentIdCorrect(string commentId, string postId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            if (comment != null)
            {
                var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == postId);
                if (post != null && comment.PostId == post.Id)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<EditCommentViewModel> ExtractCurrentComment(string commentId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            return new EditCommentViewModel
            {
                CommentId = comment.Id,
                ParentId = comment.ParentCommentId,
                Content = comment.Content,
                PostId = comment.PostId,
            };
        }

        public async Task<Tuple<string, string>> EditComment(EditCommentViewModel model)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == model.CommentId);
            if (comment != null)
            {
                comment.Content = model.SanitizedContent;
                comment.UpdatedOn = DateTime.UtcNow;
                this.db.Update(comment);
                await this.db.SaveChangesAsync();

                return Tuple.Create("Success", SuccessMessages.SuccessfullyEditedComment);
            }

            return Tuple.Create("Error", ErrorMessages.InvalidInputModel);
        }

        private async Task RemoveChildren(string i)
        {
            var children = this.db.Comments.Where(c => c.ParentCommentId == i).ToList();
            foreach (var child in children)
            {
                await this.RemoveChildren(child.Id);
                this.db.Comments.Remove(child);
            }
        }
    }
}