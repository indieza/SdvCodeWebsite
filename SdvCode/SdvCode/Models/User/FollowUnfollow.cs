// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class FollowUnfollow
    {
        [Required]
        public string PersonId { get; set; }

        [Required]
        public string FollowerId { get; set; }

        [Required]
        public bool IsFollowed { get; set; }
    }
}