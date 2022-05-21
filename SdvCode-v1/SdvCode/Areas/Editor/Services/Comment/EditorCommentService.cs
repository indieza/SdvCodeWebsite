// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Comment
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class EditorCommentService : IEditorCommentService
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly INotificationService notificationService;

        public EditorCommentService(
            ApplicationDbContext db,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService)
        {
            this.db = db;
            this.notificationHubContext = notificationHubContext;
            this.notificationService = notificationService;
        }

        public async Task<bool> ApprovedCommentById(string commentId, ApplicationUser currentUser)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (comment != null)
            {
                comment.CommentStatus = CommentStatus.Approved;

                var specialRoleIds = this.db.Roles
                        .Where(x => x.Name == Roles.Administrator.ToString() ||
                        x.Name == Roles.Editor.ToString())
                        .Select(x => x.Id)
                        .ToList();
                var specialIds = this.db.UserRoles
                    .Where(x => specialRoleIds.Contains(x.RoleId))
                    .Select(x => x.UserId)
                    .ToList();

                var postUserId = await this.db.Posts
                    .Where(x => x.Id == comment.PostId)
                    .Select(x => x.ApplicationUserId)
                    .FirstOrDefaultAsync();
                var commentUser = await this.db.Users
                    .FirstOrDefaultAsync(x => x.Id == comment.ApplicationUserId);

                var postId = await this.db.Posts
                    .Where(x => x.Id == comment.PostId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                string notificationId =
                    await this.notificationService.AddApprovedCommentNotification(commentUser, currentUser, comment.Content, postId);

                var count = await this.notificationService.GetUserNotificationsCount(commentUser.UserName);
                await this.notificationHubContext
                    .Clients
                    .User(commentUser.Id)
                    .SendAsync("ReceiveNotification", count, true);

                var notification = await this.notificationService.GetNotificationById(notificationId);
                await this.notificationHubContext.Clients.User(commentUser.Id)
                    .SendAsync("VisualizeNotification", notification);

                this.db.Comments.Update(comment);
                await this.db.SaveChangesAsync();

                var targetUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == postUserId);
                if (!specialIds.Contains(targetUser.Id))
                {
                    string notificationForApprovingId = string.Empty;

                    if (comment.ParentCommentId == null)
                    {
                        notificationForApprovingId =
                               await this.notificationService
                               .AddCommentPostNotification(targetUser, commentUser, comment.Content, postId);
                    }
                    else
                    {
                        var parentCommentUserId = await this.db.Comments
                            .Where(x => x.Id == comment.ParentCommentId)
                            .Select(x => x.ApplicationUserId)
                            .FirstOrDefaultAsync();
                        var replyUser = await this.db.Users
                            .FirstOrDefaultAsync(x => x.Id == parentCommentUserId);

                        if (parentCommentUserId != comment.ApplicationUserId)
                        {
                            notificationForApprovingId =
                                   await this.notificationService
                                   .AddCommentReplyNotification(replyUser, commentUser, comment.Content, postId);
                        }
                        else
                        {
                            notificationForApprovingId =
                                   await this.notificationService
                                   .AddCommentReplyNotification(targetUser, commentUser, comment.Content, postId);
                        }
                    }

                    var targetUserNotificationsCount = await this.notificationService.GetUserNotificationsCount(targetUser.UserName);
                    await this.notificationHubContext
                        .Clients
                        .User(targetUser.Id)
                        .SendAsync("ReceiveNotification", targetUserNotificationsCount, true);

                    var notificationForApproving = await this.notificationService.GetNotificationById(notificationForApprovingId);
                    await this.notificationHubContext.Clients.User(targetUser.Id)
                        .SendAsync("VisualizeNotification", notificationForApproving);
                }

                return true;
            }

            return false;
        }
    }
}