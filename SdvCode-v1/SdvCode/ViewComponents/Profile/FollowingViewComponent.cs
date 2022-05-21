﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile.Pagination;
    using SdvCode.Services.Profile.Pagination.Profile;
    using SdvCode.ViewModels.Pagination;
    using SdvCode.ViewModels.Pagination.Profile;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.ActivitiesComponent;

    using X.PagedList;

    public class FollowingViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileFollowingService followingService;

        public FollowingViewComponent(UserManager<ApplicationUser> userManager, IProfileFollowingService followingService)
        {
            this.userManager = userManager;
            this.followingService = followingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            List<FollowingViewModel> allFollowing = await this.followingService.ExtractFollowing(username);

            FollowingPaginationViewModel model = new FollowingPaginationViewModel
            {
                Username = username,
                Followings = allFollowing.ToPagedList(page, GlobalConstants.FollowingCountOnPage),
            };

            return this.View(model);
        }
    }
}