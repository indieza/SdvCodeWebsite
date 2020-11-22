// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class ChatImage
    {
        public ChatImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [ForeignKey(nameof(Group))]
        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [ForeignKey(nameof(ChatMessage))]
        [Required]
        public int ChatMessageId { get; set; }

        public ChatMessage ChatMessage { get; set; }
    }
}