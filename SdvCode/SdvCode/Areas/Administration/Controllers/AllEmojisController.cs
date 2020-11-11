// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Administration.Services.AllEmojis;
    using SdvCode.Areas.Administration.ViewModels.AllEmojis.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AllEmojisController : Controller
    {
        private readonly IAllEmojisService allEmojisService;

        public AllEmojisController(IAllEmojisService allEmojisService)
        {
            this.allEmojisService = allEmojisService;
        }

        public IActionResult Index()
        {
            var model = new AllEmojisViewModel
            {
                AllEmojisViewModels = this.allEmojisService.GetAllEmojis(),
            };

            return this.View(model);
        }
    }
}