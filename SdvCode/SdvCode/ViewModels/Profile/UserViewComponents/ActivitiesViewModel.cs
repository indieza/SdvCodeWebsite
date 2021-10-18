// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserViewComponents
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
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public UserActionsType Action { get; set; }

        public DateTime ActionDate { get; set; }

        public string PersonUsername { get; set; }

        public string FollowerUsername { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public string PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public UserActionsStatus ActionStatus { get; set; }
    }
}