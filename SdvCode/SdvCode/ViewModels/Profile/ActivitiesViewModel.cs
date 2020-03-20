// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class ActivitiesViewModel
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public UserActionsType Action { get; set; }

        [Required]
        public DateTime ActionDate { get; set; }

        public string PersonUsername { get; set; }

        public string FollowerUsername { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string PostId { get; set; }

        [MaxLength(150)]
        public string PostTitle { get; set; }

        [MaxLength(350)]
        public string PostContent { get; set; }
    }
}