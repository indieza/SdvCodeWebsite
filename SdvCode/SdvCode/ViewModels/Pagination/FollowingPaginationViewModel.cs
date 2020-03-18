// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.ViewModels.Profile;

    public class FollowingPaginationViewModel
    {
        public string Username { get; set; }

        public IEnumerable<FollowingViewModel> Followings { get; set; } = new HashSet<FollowingViewModel>();
    }
}