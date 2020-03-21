// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination
{
    using System.Collections.Generic;
    using SdvCode.ViewModels.Profile;

    public class BannedPostsPaginationViewModel
    {
        public string Username { get; set; }

        public IEnumerable<BannedPostsViewModel> BannedPosts { get; set; } = new HashSet<BannedPostsViewModel>();
    }
}