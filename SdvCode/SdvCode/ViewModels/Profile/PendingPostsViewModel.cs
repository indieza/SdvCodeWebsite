// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using SdvCode.Models.Blog;

    public class PendingPostsViewModel
    {
        [Required]
        public string PostId { get; set; }

        [Required]
        public string PostTitle { get; set; }

        [Required]
        public string PostContent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string AuthorUsername { get; set; }

        public string ImageUrl { get; set; }
    }
}