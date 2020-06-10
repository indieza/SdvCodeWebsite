// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Hubs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class PrivateChatHub : Hub
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly INotificationService notificationService;

        public PrivateChatHub(
            ApplicationDbContext db,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService)
        {
            this.db = db;
            this.notificationHubContext = notificationHubContext;
            this.notificationService = notificationService;
        }

        public async Task AddToGroup(string groupName, string toUsername, string fromUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var targetGroup = await this.db.Groups
                .FirstOrDefaultAsync(x => x.Name.ToLower() == groupName.ToLower());

            if (targetGroup == null)
            {
                targetGroup = new Group
                {
                    Name = groupName,
                };

                var targetToUser = new UserGroup
                {
                    ApplicationUser = toUser,
                    Group = targetGroup,
                };

                var targetFromUser = new UserGroup
                {
                    ApplicationUser = fromUser,
                    Group = targetGroup,
                };

                targetGroup.UsersGroups.Add(targetToUser);
                targetGroup.UsersGroups.Add(targetFromUser);

                this.db.Groups.Add(targetGroup);
                await this.db.SaveChangesAsync();
            }

            await this.Clients.Group(groupName).SendAsync("ReceiveMessage", fromUsername, fromImage, $"{fromUsername} has joined the group {groupName}.");
        }

        public async Task SendMessage(string fromUsername, string toUsername, string message, string group)
        {
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            this.db.ChatMessages.Add(new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
                Content = message,
            });

            var notification = new UserNotification
            {
                ApplicationUserId = fromUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = message,
                TargetUsername = toUser.UserName,
                Link = $"/PrivateChat/With/{fromUser.UserName}/Group/{group}",
                NotificationType = NotificationType.Message,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();

            await this.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, message);

            var count = await this.notificationService.GetUserNotificationsCount(toUsername);
            await this.notificationHubContext.Clients.User(toId).SendAsync("ReceiveNotification", count);

            var newNotification = await this.notificationService.GetNotificationById(notification.Id);
            await this.notificationHubContext.Clients.User(toId)
                .SendAsync("VisualizeNotification", newNotification);
        }

        public async Task ReceiveMessage(string fromUsername, string message, string group)
        {
            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            this.db.ChatMessages.Add(new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = fromUser.UserName,
                RecieverImageUrl = fromUser.ImageUrl,
                Content = message,
            });
            await this.db.SaveChangesAsync();

            await this.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, message);
        }
    }
}