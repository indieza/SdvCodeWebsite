// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.ViewModels.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class LoadMoreMessagesViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime SendedOn { get; set; }

        public string Username { get; set; }

        public string ImageUrl { get; set; }
    }
}