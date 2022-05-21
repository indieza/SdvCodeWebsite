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
    using SdvCode.Areas.Administration.Services.AddHolidayTheme;
    using SdvCode.Areas.Administration.ViewModels.AddHolidayTheme.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddHolidayThemeController : Controller
    {
        private readonly IAddHolidayThemeService addHolidayThemeService;

        public AddHolidayThemeController(IAddHolidayThemeService addHolidayThemeService)
        {
            this.addHolidayThemeService = addHolidayThemeService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddHolidayThemeInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result = await this.addHolidayThemeService.AddNewHolidayTheme(model);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.View();
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "AddHolidayTheme");
            }

            return this.View();
        }
    }
}