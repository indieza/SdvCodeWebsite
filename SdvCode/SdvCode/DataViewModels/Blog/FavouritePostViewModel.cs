// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.DataViewModels.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Users.ViewModels;

    public class FavouritePostViewModel
    {
        public string PostId { get; set; }

        public PostViewModel Post { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public bool IsFavourite { get; set; }
    }
}