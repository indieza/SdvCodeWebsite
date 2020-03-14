// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Data;
    using SdvCode.Models;
    using SdvCode.Services;
    using SdvCode.ViewModels.Profile;
    using X.PagedList;

    public class FollowersViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileService profileService;

        public FollowersViewComponent(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            List<FollowersViewModel> allFollowers = await this.profileService.ExtractFollowers(user, currentUserId);
            this.ViewBag.Followers = allFollowers.ToPagedList(page, 1);
            this.ViewBag.Username = username;
            return this.View(allFollowers);
        }
    }
}