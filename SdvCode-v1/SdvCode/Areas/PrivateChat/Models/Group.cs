﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Group
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(ChatTheme))]
        public string ChatThemeId { get; set; }

        public ChatTheme ChatTheme { get; set; }

        public ICollection<UserGroup> UsersGroups { get; set; } = new HashSet<UserGroup>();

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();
    }
}