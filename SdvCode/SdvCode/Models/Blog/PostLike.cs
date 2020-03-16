// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.Blog
{
    using System.ComponentModel.DataAnnotations;

    public class PostLike
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string PostId { get; set; }

        public bool IsLiked { get; set; }
    }
}