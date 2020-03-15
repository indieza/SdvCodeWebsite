// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile
{
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class ProfileViewModel
    {
        public ProfileTab ActiveTab { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }

        public int Page { get; set; }
    }
}