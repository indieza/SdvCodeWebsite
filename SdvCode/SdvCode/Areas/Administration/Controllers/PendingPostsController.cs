// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;
    using SdvCode.Areas.Administration.Services.PendingPosts;
    using SdvCode.Areas.Administration.ViewModels.PendingPostsViewModels;
    using SdvCode.Constraints;
    using SdvCode.MlModels.PostModels;

    [Area(GlobalConstants.AdministrationArea)]
    public class PendingPostsController : Controller
    {
        private readonly IPendingPostsService pendingPostsService;
        private readonly PredictionEnginePool<BlogPostModelInput, BlogPostModelOutput> predictionEngine;

        public PendingPostsController(
            IPendingPostsService pendingPostsService,
            PredictionEnginePool<BlogPostModelInput, BlogPostModelOutput> predictionEngine)
        {
            this.pendingPostsService = pendingPostsService;
            this.predictionEngine = predictionEngine;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<AdminPendingPostViewModel> model =
                await this.pendingPostsService.ExtractAllPendingPosts(this.predictionEngine);
            return this.View(model);
        }
    }
}