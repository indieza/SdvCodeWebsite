// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.ViewModels.PrivateChat
{
    using System.Collections;
    using System.Collections.Generic;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Areas.PrivateChat.ViewModels.ChatTheme;
    using SdvCode.Models.User;

    public class PrivateChatViewModel
    {
        public ApplicationUser FromUser { get; set; }

        public ApplicationUser ToUser { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public string GroupName { get; set; }

        public Dictionary<EmojiType, ICollection<ChatEmojiViewModel>> Emojis { get; set; } =
            new Dictionary<EmojiType, ICollection<ChatEmojiViewModel>>();

        public ChatThemeViewModel ChatThemeViewModel { get; set; } = new ChatThemeViewModel();

        public ICollection<ChatThemeViewModel> AllChatThemes { get; set; } = new HashSet<ChatThemeViewModel>();

        public ICollection<ChatStickerTypeViewModel> AllStickers { get; set; } =
            new HashSet<ChatStickerTypeViewModel>();
    }
}