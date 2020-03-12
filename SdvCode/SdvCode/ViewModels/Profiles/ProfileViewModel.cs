// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profiles
{
    using SdvCode.Models;

    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }
    }
}