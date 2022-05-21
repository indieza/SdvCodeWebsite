﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents.AllUsers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SdvCode.Constraints;
    using SdvCode.Services.Profile.Pagination;
    using SdvCode.Services.Profile.Pagination.AllUsers;
    using SdvCode.Services.Profile.Pagination.AllUsers.RecommendedUsers;
    using SdvCode.ViewModels.Pagination;
    using SdvCode.ViewModels.Pagination.AllUsers;
    using SdvCode.ViewModels.Users.ViewModels;

    using X.PagedList;

    public class RecommendedUsersViewComponent : ViewComponent
    {
        private readonly IRecommendedUsersService recommendedUsersService;

        public RecommendedUsersViewComponent(IRecommendedUsersService recommendedUsersService)
        {
            this.recommendedUsersService = recommendedUsersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page, string search)
        {
            List<AllUsersUserCardViewModel> allActivities = await this.recommendedUsersService.ExtractAllUsers(username, search);

            RecommendedUsersPaginationViewModel model = new RecommendedUsersPaginationViewModel
            {
                AllUsers = allActivities.ToPagedList(page, GlobalConstants.UsersCountOnPage),
            };

            return this.View(model);
        }
    }
}