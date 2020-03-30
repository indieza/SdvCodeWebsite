// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile
{
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class ProfileViewModel
    {
        public ProfileTab ActiveTab { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }

        public int Page { get; set; }

        public int CreatedPosts { get; set; }

        public int LikedPosts { get; set; }
    }
}