// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public string Search { get; set; }

        public IEnumerable<UserCardViewModel> UsersCards { get; set; } = new HashSet<UserCardViewModel>();
    }
}