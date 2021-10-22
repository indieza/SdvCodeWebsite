// Copyright (c) SDV Code Project. All rights reserved.
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

    public class FollowersViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileFollowersService followersService;

        public FollowersViewComponent(UserManager<ApplicationUser> userManager, IProfileFollowersService followersService)
        {
            this.userManager = userManager;
            this.followersService = followersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            List<FollowersViewModel> allFollowers = this.followersService.ExtractFollowers(user, currentUserId);

            FollowersPaginationViewModel model = new FollowersPaginationViewModel
            {
                Username = username,
                Followers = allFollowers.ToPagedList(page, GlobalConstants.FollowersCountOnPage),
            };

            return this.View(model);
        }
    }
}