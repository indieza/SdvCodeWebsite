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
            var targetComment = await this.db.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (targetComment != null)
            {
                targetComment.CommentStatus = CommentStatus.Approved;
                this.db.Comments.Update(targetComment);
                await this.db.SaveChangesAsync();

                var postUserId = await this.db.Posts
                    .Where(x => x.Id == targetComment.PostId)
                    .Select(x => x.ApplicationUserId)
                    .FirstOrDefaultAsync();

                var toUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == postUserId);
                var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == targetComment.ApplicationUserId);

                string notificationId =
                    await this.notificationService
                    .AddApprovedCommentNotification(toUser, user, targetComment.Content, targetComment.PostId);

                var count = await this.notificationService.GetUserNotificationsCount(toUser.UserName);
                await this.notificationHubContext
                    .Clients
                    .User(toUser.Id)
                    .SendAsync("ReceiveNotification", count, true);

                var notification = await this.notificationService.GetNotificationById(notificationId);
                await this.notificationHubContext.Clients.User(toUser.Id)
                    .SendAsync("VisualizeNotification", notification);

                return true;
            }

            return false;
        }
    }
}