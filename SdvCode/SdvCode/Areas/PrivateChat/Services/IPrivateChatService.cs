// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Areas.PrivateChat.Models;

    public interface IPrivateChatService
    {
        Task<ICollection<ChatMessage>> ExtractAllMessages(string group);
        Task<bool> IsUserAbleToChat(string username, HttpContext httpContext);
    }
}