// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Areas.PrivateChat.ViewModels.ChatTheme;
    using SdvCode.Areas.PrivateChat.ViewModels.PrivateChat;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.User;

    public class PrivateChatService : IPrivateChatService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<PrivateChatHub> hubContext;

        public PrivateChatService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IHubContext<PrivateChatHub> hubContext)
        {
            this.db = db;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public async Task AddUserToGroup(string groupName, string toUsername, string fromUsername)
        {
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

            await this.hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", fromUsername, fromImage, $"{fromUsername} has joined the group {groupName}.");
        }

        public async Task ChangeChatTheme(string username, string group, string themeId)
        {
            var toId = await this.db.Users
                .Where(x => x.UserName.ToUpper() == username.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            var targetTheme = await this.db.ChatThemes.FirstOrDefaultAsync(x => x.Id == themeId);
            var targetGroup = await this.db.Groups.FirstOrDefaultAsync(x => x.Name.ToUpper() == group.ToUpper());

            targetGroup.ChatThemeId = targetTheme.Id;
            this.db.Groups.Update(targetGroup);
            await this.db.SaveChangesAsync();
            await this.hubContext.Clients.User(toId).SendAsync("ReceiveThemeUpdate", targetTheme.Url);
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

        public Dictionary<EmojiType, ICollection<ChatEmojiViewModel>> GetAllEmojis()
        {
            var result = new Dictionary<EmojiType, ICollection<ChatEmojiViewModel>>();

            foreach (var emojiType in Enum.GetValues(typeof(EmojiType)))
            {
                result.Add((EmojiType)emojiType, new List<ChatEmojiViewModel>());
                var emojis = this.db.Emojis
                    .Where(x => x.EmojiType == (EmojiType)emojiType)
                    .OrderBy(x => x.Position)
                    .ToList();

                foreach (var emoji in emojis)
                {
                    result[(EmojiType)emojiType].Add(new ChatEmojiViewModel
                    {
                        Id = emoji.Id,
                        Name = emoji.Name,
                        Position = emoji.Position,
                        Url = emoji.Url,
                        SkinsUrls = this.db.EmojiSkins.Where(x => x.EmojiId == emoji.Id).OrderBy(x => x.Position).Select(x => x.Url).ToList(),
                    });
                }
            }

            return result;
        }

        public ICollection<ChatThemeViewModel> GetAllThemes()
        {
            var result = new List<ChatThemeViewModel>();
            var allThemes = this.db.ChatThemes.OrderBy(x => x.Name).ToList();

            foreach (var theme in allThemes)
            {
                result.Add(new ChatThemeViewModel
                {
                    Id = theme.Id,
                    Name = theme.Name,
                    Url = theme.Url,
                });
            }

            return result;
        }

        public ChatThemeViewModel GetGroupTheme(string group)
        {
            var targetThemeId = this.db.Groups
                .Where(x => x.Name.ToLower() == group.ToLower()).Select(x => x.ChatThemeId).FirstOrDefault();
            var targetTheme = this.db.ChatThemes.FirstOrDefault(x => x.Id == targetThemeId);
            return new ChatThemeViewModel
            {
                Id = targetTheme?.Id,
                Name = targetTheme?.Name,
                Url = targetTheme?.Url,
            };
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

        public async Task ReceiveNewMessage(string fromUsername, string message, string group)
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
                Content = new HtmlSanitizer().Sanitize(message.Trim()),
            });

            await this.db.SaveChangesAsync();
            await this.hubContext.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, new HtmlSanitizer().Sanitize(message.Trim()));
        }

        public async Task<string> SendMessageToUser(string fromUsername, string toUsername, string message, string group)
        {
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
                Content = new HtmlSanitizer().Sanitize(message.Trim()),
            };

            this.db.ChatMessages.Add(newMessage);
            await this.db.SaveChangesAsync();
            await this.hubContext.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, new HtmlSanitizer().Sanitize(message.Trim()));

            return toId;
        }
    }
}