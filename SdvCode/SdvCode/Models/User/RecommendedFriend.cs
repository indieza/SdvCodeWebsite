// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class RecommendedFriend
    {
        public RecommendedFriend()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string RecommendedUsername { get; set; }

        [MaxLength(15)]
        public string RecommendedFirstName { get; set; }

        [MaxLength(15)]
        public string RecommendedLastName { get; set; }

        [Required]
        public string RecommendedImageUrl { get; set; }

        [Required]
        public string RecommendedCoverImage { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}