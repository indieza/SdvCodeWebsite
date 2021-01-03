// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
    using MoreLinq;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Areas.PrivateChat.ViewModels.ChatTheme;
    using SdvCode.Areas.PrivateChat.ViewModels.PrivateChat;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.User;
    using SdvCode.Services.Cloud;
    using Group = SdvCode.Areas.PrivateChat.Models.Group;

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
                    .OrderByDescending(x => x.SendedOn)
                    .Take(GlobalConstants.MessagesCountPerScroll)
                    .OrderBy(x => x.SendedOn)
                    .ToList();

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
            var pattern = new Regex(@"(\d+)\.\s{1}(.*)\.?(png|jpg|jpeg)?");

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
                        Name = pattern.Match(emoji.Name).Groups[2].Value,
                        Position = emoji.Position,
                        Url = emoji.Url,
                        SkinsUrls = this.db.EmojiSkins.Where(x => x.EmojiId == emoji.Id).OrderBy(x => x.Position).Select(x => x.Url).ToList(),
                    });
                }
            }

            return result;
        }

        public ICollection<ChatStickerTypeViewModel> GetAllStickers(ApplicationUser currentUser)
        {
            var result = new List<ChatStickerTypeViewModel>();

            var stickersTypesIds = this.db.FavouriteStickers
                .Where(x => x.ApplicationUserId == currentUser.Id && x.IsFavourite)
                .Select(x => x.StickerTypeId)
                .ToList();

            var stickersTypes = this.db.StickerTypes
                .Where(x => stickersTypesIds.Contains(x.Id))
                .OrderBy(x => x.Position)
                .ToList();

            foreach (var stickerType in stickersTypes)
            {
                var targetStickerType = new ChatStickerTypeViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                    Position = stickerType.Position,
                    Url = stickerType.Url,
                    Stickers = new HashSet<ChatStickerViewModel>(),
                };

                var stickers = this.db.Stickers
                    .Where(x => x.StickerTypeId == stickerType.Id)
                    .OrderBy(x => x.Position).ToList();

                foreach (var sticker in stickers)
                {
                    targetStickerType.Stickers.Add(new ChatStickerViewModel
                    {
                        Id = sticker.Id,
                        Name = sticker.Name,
                        Position = sticker.Position,
                        Url = sticker.Url,
                    });
                }

                result.Add(targetStickerType);
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

        public async Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount)
        {
            var result = new List<LoadMoreMessagesViewModel>();

            var targetGroup = await this.db.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

            if (targetGroup != null)
            {
                var messages = this.db.ChatMessages
                    .Where(x => x.GroupId == targetGroup.Id)
                    .OrderByDescending(x => x.SendedOn)
                    .Skip(messagesSkipCount)
                    .Take(GlobalConstants.MessagesCountPerScroll)
                    .OrderBy(x => x.SendedOn)
                    .ToList();

                foreach (var message in messages)
                {
                    var currentMessageModel = new LoadMoreMessagesViewModel
                    {
                        Id = message.Id,
                        Content = message.Content,
                        SendedOn = message.SendedOn,
                    };

                    var messageUser = await this.db.Users
                        .FirstOrDefaultAsync(x => x.Id == message.ApplicationUserId);

                    currentMessageModel.Username = messageUser.UserName;
                    currentMessageModel.ImageUrl = messageUser.ImageUrl;

                    result.Add(currentMessageModel);
                }
            }

            return result;
        }

        public async Task ReceiveNewMessage(string fromUsername, string message, string group)
        {
            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.hubContext.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, message.Trim());
        }

        public async Task ReceiveStickerMessage(string fromUsername, string group, string stickerUrl)
        {
            var fromUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var message = $"<span><img style=\"width: 127px;\" src=\"{stickerUrl}\" /></span>";

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

            await this.DeleteOldMessage(group);

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

        public async Task<SendFilesResponseViewModel> SendMessageWitFilesToUser(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message)
        {
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.DeleteOldMessage(group);

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
            };

            StringBuilder messageContent = new StringBuilder();

            if (message != null)
            {
                messageContent.AppendLine($"{new HtmlSanitizer().Sanitize(message.Trim())}<hr style=\"margin-bottom: 8px !important;\" />");
            }

            StringBuilder imagesContent = new StringBuilder();
            StringBuilder filesContent = new StringBuilder();

            var imagesCount = files
                .Where(x => x.ContentType
                    .Contains("image", StringComparison.CurrentCultureIgnoreCase))
                .Count();

            var result = new SendFilesResponseViewModel();

            if (imagesCount > 0)
            {
                await this.hubContext.Clients.User(fromId).SendAsync("UpdateImagesUploadCount", imagesCount);
                result.HaveImages = true;
            }

            var filesCount = files
                .Where(x => !x.ContentType
                    .Contains("image", StringComparison.CurrentCultureIgnoreCase))
                .Count();

            if (filesCount > 0)
            {
                await this.hubContext.Clients.User(fromId).SendAsync("UpdateFilesUploadCount", filesCount);
                result.HaveFiles = true;
            }

            foreach (var file in files)
            {
                var chatFile = new ChatImage
                {
                    ChatMessageId = newMessage.Id,
                    GroupId = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()).Id,
                };

                string fileUrl = string.Empty;

                if (file.ContentType.Contains("image", StringComparison.CurrentCultureIgnoreCase))
                {
                    fileUrl = await ApplicationCloudinary.UploadImage(
                                this.cloudinary,
                                file,
                                string.Format(GlobalConstants.ChatFileName, chatFile.Id),
                                GlobalConstants.PrivateChatImagesFolder);
                    chatFile.Name = string.Format(GlobalConstants.ChatFileName, chatFile.Id);

                    imagesCount--;
                    await this.hubContext.Clients.User(fromId).SendAsync("UpdateImagesUploadCount", imagesCount);

                    imagesContent.AppendLine($"<span onclick=\"zoomChatImage('{fileUrl}')\"><img src=\"{fileUrl}\" style=\"margin-right: 10px; width: 27px; height: 35px; margin-top: 5px;\"></span>");
                }
                else
                {
                    var fileExtension = Path.GetExtension(file.FileName);

                    fileUrl = await ApplicationCloudinary.UploadImage(
                                this.cloudinary,
                                file,
                                string.Format(GlobalConstants.ChatFileName, $"{chatFile.Id}") + fileExtension,
                                GlobalConstants.PrivateChatImagesFolder);
                    chatFile.Name =
                        string.Format(GlobalConstants.ChatFileName, $"{chatFile.Id}{fileExtension}");

                    filesCount--;
                    await this.hubContext.Clients.User(fromId).SendAsync("UpdateFilesUploadCount", filesCount);

                    string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                    double fileLength = file.Length;
                    int order = 0;
                    while (fileLength >= 1024 && order < sizes.Length - 1)
                    {
                        order++;
                        fileLength /= 1024;
                    }

                    string fileSize = string.Format("{0:0.##} {1}", fileLength, sizes[order]);

                    filesContent.AppendLine($"<p><a href=\"{fileUrl}\"><i class=\"fas fa-download\"></i> {file.FileName} - ({fileSize})</a></p>");
                }

                chatFile.Url = fileUrl;
                newMessage.ChatImages.Add(chatFile);
            }

            if (imagesCount > 0)
            {
                await this.hubContext.Clients.User(fromId).SendAsync("UpdateImagesUploadCount", imagesCount);
            }

            if (filesCount > 0)
            {
                await this.hubContext.Clients.User(fromId).SendAsync("UpdateFilesUploadCount", filesCount);
            }

            if (imagesContent.Length == 0)
            {
                messageContent.AppendLine(filesContent.ToString().Trim());
            }
            else
            {
                messageContent.AppendLine(imagesContent.ToString().Trim());

                if (filesContent.Length != 0)
                {
                    messageContent.AppendLine("<hr style=\"margin-bottom: 8px !important;\" />");
                    messageContent.AppendLine(filesContent.ToString().Trim());
                }
            }

            newMessage.Content = messageContent.ToString().Trim();

            this.db.ChatMessages.Add(newMessage);
            await this.db.SaveChangesAsync();
            await this.hubContext
                .Clients
                .User(toId)
                .SendAsync("ReceiveMessage", fromUsername, fromImage, messageContent.ToString().Trim());
            await this.ReceiveNewMessage(fromUsername, messageContent.ToString().Trim(), group);

            return result;
        }

        public async Task SendStickerMessageToUser(string fromUsername, string toUsername, string group, string stickerUrl)
        {
            var toUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.DeleteOldMessage(group);

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.db.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
                Content = $"<span><img style=\"width: 127px;\" src=\"{stickerUrl}\" /></span>",
            };

            this.db.ChatMessages.Add(newMessage);
            await this.db.SaveChangesAsync();
            await this.hubContext.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, newMessage.Content);
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

        private async Task DeleteOldMessage(string group)
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
                    var oldMessages = messages.Take(messages.Count - GlobalConstants.SavedChatMessagesCount);

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
                }
            }
        }
    }
}