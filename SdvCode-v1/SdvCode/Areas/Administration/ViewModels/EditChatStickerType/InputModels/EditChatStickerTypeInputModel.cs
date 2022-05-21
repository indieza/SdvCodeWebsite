// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditChatStickerType.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class EditChatStickerTypeInputModel
    {
        [Display(Name = "Sticker Type Name")]
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        [Display(Name = "Sticker Type Name")]
        public string Name { get; set; }

        [Display(Name = "Sticker Type Image")]
        public IFormFile Image { get; set; }
    }
}