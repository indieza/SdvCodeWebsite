// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Post.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;

    public class EditPostInputModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Tags")]
        public ICollection<string> TagsNames { get; set; } = new HashSet<string>();

        public ICollection<string> Categories { get; set; } = new HashSet<string>();

        public ICollection<string> Tags { get; set; } = new HashSet<string>();
    }
}