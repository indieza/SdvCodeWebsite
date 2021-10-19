// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewComponents;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;

    public class PendingPostsPaginationViewModel
    {
        public string Username { get; set; }

        public IEnumerable<PendingPostViewModel> PendingPosts { get; set; } = new HashSet<PendingPostViewModel>();
    }
}