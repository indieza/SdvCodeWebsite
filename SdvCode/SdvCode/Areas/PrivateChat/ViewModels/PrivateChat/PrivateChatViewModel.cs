// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.ViewModels.PrivateChat
{
    using SdvCode.Models.User;

    public class PrivateChatViewModel
    {
        public ApplicationUser FromUser { get; set; }

        public ApplicationUser ToUser { get; set; }
    }
}