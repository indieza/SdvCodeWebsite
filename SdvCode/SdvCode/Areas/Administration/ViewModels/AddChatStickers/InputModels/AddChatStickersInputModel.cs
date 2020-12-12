// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddChatStickers.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class AddChatStickersInputModel
    {
        [Required]
        [Display(Name = "Sticker Type Name")]
        public string StickerTypeId { get; set; }

        [Required]
        [Display(Name = "Stickers Images")]
        public IFormFile[] Images { get; set; }
    }
}