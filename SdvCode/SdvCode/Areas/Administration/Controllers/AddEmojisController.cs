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
    using SdvCode.Areas.Administration.Services.AddEmojis;
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddEmojisController : Controller
    {
        private readonly IAddEmojisService addEmojisService;

        public AddEmojisController(IAddEmojisService addEmojisService)
        {
            this.addEmojisService = addEmojisService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmojis(AddEmojisInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                string mesage = await this.addEmojisService.AddEmojis(model);
                this.TempData["Success"] = mesage;
                return this.RedirectToAction("Index", "AddEmojis");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "AddEmojis", model);
            }
        }
    }
}