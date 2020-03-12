// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models
{
    public class FollowUnfollow
    {
        public string PersonId { get; set; }

        public string FollowerId { get; set; }

        public bool IsFollowed { get; set; }
    }
}