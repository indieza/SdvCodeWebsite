// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class DeleteChatThemeInputModel
    {
        [Required]
        [Display(Name = "Theme Name")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Theme Name")]
        public string Name { get; set; }
    }
}