// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    public class UserCardViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }

        public int Activities { get; set; }

        public bool HasFollowed { get; set; }
    }
}