// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Dashboard
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Areas.Administration.ViewModels.DashboardViewModels;

    public interface IDashboardService
    {
        DashboardViewModel GetDashboardInformation();

        Task<IdentityResult> CreateRole(string role);

        Task<bool> IsAddedUserInRole(string inputRole, string inputUsername);

        Task<bool> RemoveUserFromRole(string username, string role);

        Task<bool> SyncFollowUnfollow();
    }
}