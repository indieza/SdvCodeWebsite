// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Home
{
    using System.Collections.Generic;
    using SdvCode.Models.User;

    public class HomeViewModel
    {
        public int TotalRegisteredUsers { get; set; }

        public int TotalBlogPosts { get; set; }

        public int TotalShopProducts { get; set; }

        public ICollection<ApplicationUser> Administrators { get; set; } = new HashSet<ApplicationUser>();
    }
}