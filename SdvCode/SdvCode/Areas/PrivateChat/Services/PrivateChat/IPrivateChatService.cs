// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Areas.PrivateChat.ViewModels.ChatTheme;
    using SdvCode.Areas.PrivateChat.ViewModels.PrivateChat;
    using SdvCode.Models.User;

    public interface IPrivateChatService
    {
        Task<ICollection<ChatMessage>> ExtractAllMessages(string group);

        Task<bool> IsUserAbleToChat(string username, string group, ApplicationUser user);

        Task AddUserToGroup(string groupName, string toUsername, string fromUsername);

        Task<string> SendMessageToUser(string fromUsername, string toUsername, string message, string group);

        Task ReceiveNewMessage(string fromUsername, string message, string group);

        Dictionary<EmojiType, ICollection<ChatEmojiViewModel>> GetAllEmojis();

        ICollection<ChatThemeViewModel> GetAllThemes();

        ChatThemeViewModel GetGroupTheme(string group);

        Task ChangeChatTheme(string username, string group, string themeId);

        Task<SendFilesResponseViewModel> SendMessageWitFilesToUser(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message);

        Task UserType(string fromUsername, string toUsername, string fromUserImageUrl);

        Task UserStopType(string toUsername);

        ICollection<ChatStickerTypeViewModel> GetAllStickers(ApplicationUser currentUser);

        Task SendStickerMessageToUser(string fromUsername, string toUsername, string group, string stickerUrl);

        Task ReceiveStickerMessage(string fromUsername, string group, string stickerUrl);

        Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount, ApplicationUser currentUser);
        ICollection<QuickChatReplyViewModel> GetAllQuickReplies(ApplicationUser currentUser);
    }
}