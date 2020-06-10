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
    using SdvCode.Models.User;

    public interface IPrivateChatService
    {
        Task<ICollection<ChatMessage>> ExtractAllMessages(string group);

        Task<bool> IsUserAbleToChat(string username, string group, ApplicationUser user);

        Task AddUserToGroup(string groupName, string toUsername, string fromUsername);

        Task<string> SendMessageToUser(string fromUsername, string toUsername, string message, string group);
        Task ReceiveNewMessage(string fromUsername, string message, string group);
    }
}