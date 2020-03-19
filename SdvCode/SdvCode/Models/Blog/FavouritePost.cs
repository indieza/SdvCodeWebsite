// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.Blog
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.User;

    public class FavouritePost
    {
        [Required]
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public bool IsFavourite { get; set; }
    }
}