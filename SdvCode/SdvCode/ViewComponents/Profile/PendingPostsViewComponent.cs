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
    using SdvCode.ViewModels.Profile.UserViewComponents;

    using X.PagedList;

    public class PendingPostsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfilePendingPostsService pendingPostsService;

        public PendingPostsViewComponent(UserManager<ApplicationUser> userManager, IProfilePendingPostsService pendingPostsService)
        {
            this.userManager = userManager;
            this.pendingPostsService = pendingPostsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            List<PendingPostsViewModel> allPendingPosts = await this.pendingPostsService.ExtractPendingPosts(user, currentUserId);

            PendingPostsPaginationViewModel model = new PendingPostsPaginationViewModel
            {
                Username = username,
                PendingPosts = allPendingPosts.ToPagedList(page, GlobalConstants.PendingPostsCountOnPage),
            };

            return this.View(model);
        }
    }
}