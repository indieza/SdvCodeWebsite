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
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models;
    using SdvCode.Services.Profile.Pagination;
    using SdvCode.ViewModels.Pagination;
    using SdvCode.ViewModels.Profile;
    using X.PagedList;

    public class ActivitiesViewComponent : ViewComponent
    {
        private readonly IProfileActivitiesService activitiesService;

        public ActivitiesViewComponent(IProfileActivitiesService activitiesService)
        {
            this.activitiesService = activitiesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            List<ActivitiesViewModel> allActivities = await this.activitiesService.ExtractActivities(username);

            ActivitiesPaginationViewModel model = new ActivitiesPaginationViewModel
            {
                Username = username,
                Activities = allActivities.ToPagedList(page, GlobalConstants.UsersActivitiesCountOnPage),
            };

            return this.View(model);
        }
    }
}