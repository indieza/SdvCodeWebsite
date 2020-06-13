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
    using SdvCode.Areas.PrivateChat.Services.PrivateChat;
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class PrivateChatHub : Hub
    {
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly INotificationService notificationService;
        private readonly IPrivateChatService privateChatService;

        public PrivateChatHub(
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService,
            IPrivateChatService privateChatService)
        {
            this.notificationHubContext = notificationHubContext;
            this.notificationService = notificationService;
            this.privateChatService = privateChatService;
        }

        public async Task AddToGroup(string groupName, string toUsername, string fromUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.privateChatService.AddUserToGroup(groupName, toUsername, fromUsername);
        }

        public async Task SendMessage(string fromUsername, string toUsername, string message, string group)
        {
            string toId =
                await this.privateChatService.SendMessageToUser(fromUsername, toUsername, message, group);
            string notificationId =
                await this.notificationService.AddMessageNotification(fromUsername, toUsername, message, group);

            var count = await this.notificationService.GetUserNotificationsCount(toUsername);
            await this.notificationHubContext.Clients.User(toId).SendAsync("ReceiveNotification", count, true);

            var notification = await this.notificationService.GetNotificationById(notificationId);
            await this.notificationHubContext.Clients.User(toId)
                .SendAsync("VisualizeNotification", notification);
        }

        public async Task ReceiveMessage(string fromUsername, string message, string group)
        {
            await this.privateChatService.ReceiveNewMessage(fromUsername, message, group);
        }
    }
}