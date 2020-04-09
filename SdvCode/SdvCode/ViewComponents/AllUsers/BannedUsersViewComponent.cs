// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents.AllUsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Services.Profile.Pagination;
    using SdvCode.Services.Profile.Pagination.AllUsers;
    using SdvCode.ViewModels.Pagination;
    using SdvCode.ViewModels.Pagination.AllUsers;
    using SdvCode.ViewModels.Users.ViewModels;
    using X.PagedList;

    public class BannedUsersViewComponent : ViewComponent
    {
        private readonly IBannedUsersService bannedUsersService;

        public BannedUsersViewComponent(IBannedUsersService bannedUsersService)
        {
            this.bannedUsersService = bannedUsersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page, string search)
        {
            List<UserCardViewModel> allActivities = await this.bannedUsersService.ExtractAllUsers(username, search);

            BannedUsersPaginationViewModel model = new BannedUsersPaginationViewModel
            {
                AllUsers = allActivities.ToPagedList(page, GlobalConstants.UsersCountOnPage),
            };

            return this.View(model);
        }
    }
}