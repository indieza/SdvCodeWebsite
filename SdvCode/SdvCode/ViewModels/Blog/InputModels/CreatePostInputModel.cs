// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class CreatePostInputModel
    {
        [Required]
        [MaxLength(150)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Tags")]
        public ICollection<string> TagsNames { get; set; } = new HashSet<string>();
    }
}