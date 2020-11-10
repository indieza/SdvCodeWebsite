// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditEmojiViewModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class EditEmojiInputModel
    {
        [Required]
        [Display(Name = "Emoji Name")]
        public string Id { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = "Emoji Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Emoji Unicode")]
        public string Code { get; set; }

        [Required]
        [EnumDataType(typeof(EmojiType))]
        [Display(Name = "Emoji Type")]
        public EmojiType EmojiType { get; set; }
    }
}