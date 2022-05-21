﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AllEmojis.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class EmojiViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int Position { get; set; }

        public EmojiType EmojiType { get; set; }

        public ICollection<string> SkinsUrls { get; set; } = new HashSet<string>();
    }
}