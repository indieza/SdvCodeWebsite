// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddEmojiWithSkin.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class AddEmojiWithSkinInputModel
    {
        [Required]
        [MaxLength(120)]
        [Display(Name = "Emoji Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Emoji Image")]
        public IFormFile Image { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        [EnumDataType(typeof(EmojiType))]
        [Display(Name = "Emoji Type")]
        public EmojiType EmojiType { get; set; }

        [Required]
        [Display(Name = "Emoji Skins")]
        public ICollection<IFormFile> ImageSkins { get; set; } = new HashSet<IFormFile>();
    }
}