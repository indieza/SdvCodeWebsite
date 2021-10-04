// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Ganss.XSS;

    using Microsoft.AspNetCore.Http;

    using SdvCode.ApplicationAttributes;

    public class CreatePostInputModel
    {
        [Required]
        [MaxLength(150)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        [Required]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Tags")]
        public ICollection<string> TagsNames { get; set; } = new HashSet<string>();

        [Display(Name = "Post Images")]
        public ICollection<IFormFile> PostImages { get; set; } = new HashSet<IFormFile>();
    }
}