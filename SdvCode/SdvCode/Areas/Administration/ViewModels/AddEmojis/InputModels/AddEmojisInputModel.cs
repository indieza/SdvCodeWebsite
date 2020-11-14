// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class AddEmojisInputModel
    {
        [Required]
        [Display(Name = "Emojis Type")]
        [EnumDataType(typeof(EmojiType))]
        public EmojiType EmojiType { get; set; }

        [Required]
        [Display(Name = "Emojis Images")]
        public ICollection<IFormFile> Images { get; set; } = new HashSet<IFormFile>();
    }
}