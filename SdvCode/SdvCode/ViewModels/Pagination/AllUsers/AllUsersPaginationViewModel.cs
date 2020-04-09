// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Pagination.AllUsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.ViewModels.Users.ViewModels;

    public class AllUsersPaginationViewModel
    {
        public IEnumerable<UserCardViewModel> AllUsers { get; set; } = new HashSet<UserCardViewModel>();
    }
}