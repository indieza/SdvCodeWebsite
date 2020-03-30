// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class PrivateChatHub : Hub
    {
        private readonly ApplicationDbContext db;

        public PrivateChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddToGroup(string groupName, string toUsername, string fromUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl ?? GlobalConstants.NoAvatarImageLocation;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl ?? GlobalConstants.NoAvatarImageLocation;

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
            var toImage = toUser.ImageUrl ?? GlobalConstants.NoAvatarImageLocation;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl ?? GlobalConstants.NoAvatarImageLocation;

            this.db.ChatMessages.Add(new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
                Content = message,
            });
            await this.db.SaveChangesAsync();

            await this.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, message);
        }

        public async Task ReceiveMessage(string fromUsername, string message, string group)
        {
            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl ?? GlobalConstants.NoAvatarImageLocation;

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