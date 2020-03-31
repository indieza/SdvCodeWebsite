// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class PrivateChatService : IPrivateChatService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public PrivateChatService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ICollection<ChatMessage>> ExtractAllMessages(string group)
        {
            var targetGroup = await this.db.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

            if (targetGroup != null)
            {
                var messages = this.db.ChatMessages
                    .Where(x => x.GroupId == targetGroup.Id)
                    .OrderBy(x => x.SendedOn)
                    .ToList();

                if (messages.Count > GlobalConstants.SavedChatMessagesCount)
                {
                    var oldMessages = messages.Take(GlobalConstants.SavedChatMessagesCount);
                    this.db.ChatMessages.RemoveRange(oldMessages);
                    await this.db.SaveChangesAsync();

                    messages = this.db.ChatMessages
                        .Where(x => x.GroupId == targetGroup.Id)
                        .OrderBy(x => x.SendedOn)
                        .ToList();
                }

                foreach (var message in messages)
                {
                    message.ApplicationUser = await this.db.Users
                        .FirstOrDefaultAsync(x => x.Id == message.ApplicationUserId);
                }

                return messages;
            }

            return null;
        }

        public async Task<bool> IsUserAbleToChat(string username, string group, ApplicationUser currentUser)
        {
            var targetUser = this.db.Users.FirstOrDefault(x => x.UserName == username);
            var groupUsers = new List<string>() { currentUser.UserName, targetUser.UserName };
            var targetGroupName = string.Join(GlobalConstants.ChatGroupNameSeparator, groupUsers.OrderBy(x => x));

            if (targetGroupName != group)
            {
                return false;
            }

            if (await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRole))
            {
                return true;
            }

            if (currentUser.UserName == username)
            {
                return false;
            }

            if (currentUser.IsBlocked &&
                await this.userManager.IsInRoleAsync(targetUser, GlobalConstants.AdministratorRole))
            {
                return true;
            }

            if (currentUser.IsBlocked)
            {
                return false;
            }

            return true;
        }
    }
}