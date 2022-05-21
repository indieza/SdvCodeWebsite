// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorStrap;

    public class ProfileRoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int RoleLevel { get; set; }

        public string Description { get; set; }
    }
}