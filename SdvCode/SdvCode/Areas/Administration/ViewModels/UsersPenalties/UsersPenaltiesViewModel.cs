// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.UsersPenalties
{
    using System.Collections.Generic;

    public class UsersPenaltiesViewModel
    {
        public ICollection<string> BlockedUsernames { get; set; } = new HashSet<string>();

        public ICollection<string> NotBlockedUsernames { get; set; } = new HashSet<string>();
    }
}