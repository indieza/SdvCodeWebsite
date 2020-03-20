// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Ganss.XSS;

    public class AddCategoryInputModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [MaxLength(550)]
        [Display(Name = "Category description")]
        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);
    }
}