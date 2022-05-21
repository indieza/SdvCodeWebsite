// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.PendingComments;
    using SdvCode.Areas.Administration.ViewModels.PendingComments;
    using SdvCode.Constraints;
    using SdvCode.MlModels.CommentModels;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class PendingCommentsController : Controller
    {
        private readonly IPendingCommentsService pendingCommentsService;
        private readonly PredictionEnginePool<BlogCommentModelInput, BlogCommentModelOutput> predictionEngine;

        public PendingCommentsController(
            IPendingCommentsService pendingCommentsService,
            PredictionEnginePool<BlogCommentModelInput, BlogCommentModelOutput> predictionEngine)
        {
            this.pendingCommentsService = pendingCommentsService;
            this.predictionEngine = predictionEngine;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<AdminPendingCommentViewModel> model =
                await this.pendingCommentsService.ExtractAllPendingComments(this.predictionEngine);
            return this.View(model);
        }
    }
}