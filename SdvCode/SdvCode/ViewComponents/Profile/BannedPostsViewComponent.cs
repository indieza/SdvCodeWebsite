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
    using SdvCode.Models.User;
    using SdvCode.Services.Profile.Pagination;
    using SdvCode.Services.Profile.Pagination.Profile;
    using SdvCode.ViewModels.Pagination;
    using SdvCode.ViewModels.Pagination.Profile;
    using SdvCode.ViewModels.Profile;
    using X.PagedList;

    public class BannedPostsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileBannedPostsService bannedPostsService;

        public BannedPostsViewComponent(UserManager<ApplicationUser> userManager, IProfileBannedPostsService bannedPostsService)
        {
            this.userManager = userManager;
            this.bannedPostsService = bannedPostsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            List<BannedPostsViewModel> allBannedPosts = await this.bannedPostsService.ExtractBannedPosts(user, currentUserId);

            BannedPostsPaginationViewModel model = new BannedPostsPaginationViewModel
            {
                Username = username,
                BannedPosts = allBannedPosts.ToPagedList(page, GlobalConstants.BannedPostsCountOnPage),
            };

            return this.View(model);
        }
    }
}