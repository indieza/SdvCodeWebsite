// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using SdvCode.Models.Enums;

    public class UserAction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public UserActionsType Action { get; set; }

        [Required]
        public DateTime ActionDate { get; set; }

        public string PersonUsername { get; set; }

        public string PersonProfileImageUrl { get; set; }

        public string FollowerUsername { get; set; }

        public string FollowerProfileImageUrl { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string PostId { get; set; }
    }
}