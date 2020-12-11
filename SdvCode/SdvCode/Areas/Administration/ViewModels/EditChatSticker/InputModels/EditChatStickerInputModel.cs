// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditChatSticker.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class EditChatStickerInputModel
    {
        [Required]
        [Display(Name = "Sticker Name")]
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        [Display(Name = "Sticker Name")]
        public string Name { get; set; }

        [Display(Name = "Sticker Image")]
        public IFormFile Image { get; set; }

        [Required]
        [Display(Name = "Sticker Type Name")]
        public string StickerTypeId { get; set; }
    }
}