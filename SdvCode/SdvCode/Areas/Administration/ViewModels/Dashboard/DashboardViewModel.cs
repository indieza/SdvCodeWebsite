// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.Dashboard
{
    using System.Collections.Generic;

    public class DashboardViewModel
    {
        public int TotalUsersCount { get; set; }

        public int TotalBlogPosts { get; set; }

        public int TotalBannedUsers { get; set; }

        public int TotalUsersInAdminRole { get; set; }

        public int TotalShopProducts { get; set; }

        public int TotalOrdersCount { get; set; }

        public ICollection<string> Usernames { get; set; } = new HashSet<string>();
    }
}