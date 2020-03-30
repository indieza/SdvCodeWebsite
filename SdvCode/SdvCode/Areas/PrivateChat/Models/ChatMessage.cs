// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using SdvCode.Models.User;

    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(550)]
        public string Content { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }

        public string RecieverImageUrl { get; set; }

        [Required]
        public DateTime SendedOn { get; set; }
    }
}