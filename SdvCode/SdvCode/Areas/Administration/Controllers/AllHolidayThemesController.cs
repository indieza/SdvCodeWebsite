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
    using SdvCode.Areas.Administration.Services.AllHolidayThemes;
    using SdvCode.Areas.Administration.ViewModels.AllHolidayThemes.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AllHolidayThemesController : Controller
    {
        private readonly IAllHolidayThemesService allHolidayThemesService;

        public AllHolidayThemesController(IAllHolidayThemesService allHolidayThemesService)
        {
            this.allHolidayThemesService = allHolidayThemesService;
        }

        public IActionResult Index()
        {
            ICollection<AllHolidayThemesViewModel> model = this.allHolidayThemesService.GetAllHolidayThemes();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeThemeStatus(string id, bool status)
        {
            Tuple<bool, string> result = await this.allHolidayThemesService.ChangeHolidayThemeStatus(id, status);
            if (!result.Item1)
            {
                this.TempData["Error"] = result.Item2;
            }
            else
            {
                this.TempData["Success"] = result.Item2;
            }

            return this.RedirectToAction("Index", "AllHolidayThemes");
        }
    }
}