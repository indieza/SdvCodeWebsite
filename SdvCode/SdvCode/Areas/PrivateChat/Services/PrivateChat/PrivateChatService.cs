// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
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
    using SdvCode.Services.Cloud;

    public class PrivateChatService : IPrivateChatService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<PrivateChatHub> hubContext;
        private readonly Cloudinary cloudinary;

        public PrivateChatService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IHubContext<PrivateChatHub> hubContext,
            Cloudinary cloudinary)
        {
            this.db = db;
            this.userManager = userManager;
            this.hubContext = hubContext;
            this.cloudinary = cloudinary;
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

                    foreach (var oldMessageId in oldMessages.Select(x => x.Id).ToList())
                    {
                        var oldImages = this.db.ChatImages.Where(x => x.ChatMessageId == oldMessageId).ToList();

                        foreach (var oldImage in oldImages)
                        {
                            ApplicationCloudinary.DeleteImage(
                                this.cloudinary,
                                oldImage.Name,
                                GlobalConstants.PrivateChatImagesFolder);
                        }

                        this.db.ChatImages.RemoveRange(oldImages);
                    }

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

            await this.hubContext.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, message.Trim());
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

        public async Task<string> SendMessageWitImagesToUser(
            IList<IFormFile> files, string group, string toUsername, string fromUsername, string message)
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
            };

            string messageContent =
                message == null ? string.Empty : $"{new HtmlSanitizer().Sanitize(message.Trim())}<hr style=\"margin-bottom: 8px !important;\" />";

            var count = files.Count;

            foreach (var file in files)
            {
                var chatImage = new ChatImage
                {
                    ChatMessageId = newMessage.Id,
                    GroupId = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()).Id,
                };

                string imageUrl = await ApplicationCloudinary.UploadImage(
                    this.cloudinary,
                    file,
                    string.Format(GlobalConstants.ChatFileName, chatImage.Id),
                    GlobalConstants.PrivateChatImagesFolder);

                chatImage.Url = imageUrl;
                chatImage.Name = string.Format(GlobalConstants.ChatFileName, chatImage.Id);
                newMessage.ChatImages.Add(chatImage);

                messageContent +=
                    $"<span onclick=\"zoomChatImage('{imageUrl}')\"><img src=\"{imageUrl}\" style=\"margin-right: 10px; width: 27px; height: 35px; margin-top: 5px;\"></span>";

                await this.hubContext.Clients.User(fromId).SendAsync("UpdateFilesUploadCount", count);
                count--;
            }

            newMessage.Content = messageContent;

            this.db.ChatMessages.Add(newMessage);
            await this.db.SaveChangesAsync();
            await this.hubContext
                .Clients
                .User(toId)
                .SendAsync("ReceiveMessage", fromUsername, fromImage, messageContent.Trim());
            return messageContent;
        }

        public async Task UserStopType(string toUsername)
        {
            var toId = await this.db.Users
                .Where(x => x.UserName.ToUpper() == toUsername.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            await this.hubContext
                .Clients
                .User(toId)
                .SendAsync("VisualizeUserStopType");
        }

        public async Task UserType(string fromUsername, string toUsername, string fromUserImageUrl)
        {
            var toId = await this.db.Users
                .Where(x => x.UserName.ToUpper() == toUsername.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            await this.hubContext
                .Clients
                .User(toId)
                .SendAsync("VisualizeUserType", fromUsername, fromUserImageUrl);
        }
    }
}