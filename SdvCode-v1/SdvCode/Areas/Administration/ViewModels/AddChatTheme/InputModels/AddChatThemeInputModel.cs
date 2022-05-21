﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddChatTheme.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class AddChatThemeInputModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Theme Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Theme Image")]
        public IFormFile Image { get; set; }
    }
}