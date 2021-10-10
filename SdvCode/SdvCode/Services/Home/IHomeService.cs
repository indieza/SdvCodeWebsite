// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Home;

    public interface IHomeService
    {
        int GetRegisteredUsersCount();

        Task<IdentityResult> CreateRole(string role);

        Task<ICollection<ApplicationUser>> GetAllAdministrators();

        ICollection<LatestPostViewModel> GetLatestPosts();

        int GetPostsCount();

        int GetPorductsCount();

        Task<ICollection<string>> GetHolidayThemeIcons();
    }
}