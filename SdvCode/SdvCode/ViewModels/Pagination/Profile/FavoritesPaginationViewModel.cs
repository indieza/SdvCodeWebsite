// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;

    public class FavoritesPaginationViewModel
    {
        public string Username { get; set; }

        public IEnumerable<FavouritePostViewModel> Favorites { get; set; } = new HashSet<FavouritePostViewModel>();
    }
}