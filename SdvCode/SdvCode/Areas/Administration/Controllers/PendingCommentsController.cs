// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.PendingComments;
    using SdvCode.Areas.Administration.ViewModels.PendingCommentsViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    public class PendingCommentsController : Controller
    {
        private readonly IPendingCommentsService pendingCommentsService;

        public PendingCommentsController(IPendingCommentsService pendingCommentsService)
        {
            this.pendingCommentsService = pendingCommentsService;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<AdminPendingCommentViewModel> model =
                await this.pendingCommentsService.ExtractAllPendingComments();
            return this.View(model);
        }
    }
}