// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.ViewModels
{
    using System.Collections.Generic;
    using SdvCode.Models.Enums;

    public class UsersViewModel
    {
        public int Page { get; set; }

        public AllUsersTab ActiveTab { get; set; }

        public string Search { get; set; }
    }
}