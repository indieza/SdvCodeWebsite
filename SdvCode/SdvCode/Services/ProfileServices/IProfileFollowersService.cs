// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.ProfileServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;

    public interface IProfileFollowersService
    {
        Task<List<FollowersViewModel>> ExtractFollowers(ApplicationUser user, string currentUserId);
    }
}