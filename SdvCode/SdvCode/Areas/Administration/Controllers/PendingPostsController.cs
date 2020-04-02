// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services.PendingPosts;
    using SdvCode.Areas.Administration.ViewModels.PendingPostsViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    public class PendingPostsController : Controller
    {
        private readonly IPendingPostsService pendingPostsService;

        public PendingPostsController(IPendingPostsService pendingPostsService)
        {
            this.pendingPostsService = pendingPostsService;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<AdminPendingPostViewModel> model = await this.pendingPostsService.ExtractAllPendingPosts();
            return this.View(model);
        }
    }
}