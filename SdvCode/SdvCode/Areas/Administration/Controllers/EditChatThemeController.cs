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
    using SdvCode.Areas.Administration.Services.EditChatTheme;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class EditChatThemeController : Controller
    {
        private readonly IEditChatThemeService editChatThemeService;

        public EditChatThemeController(IEditChatThemeService editChatThemeService)
        {
            this.editChatThemeService = editChatThemeService;
        }

        public IActionResult Index()
        {
            var model = new EditChatThemeBaseModel
            {
                EditChatThemeInput = new EditChatThemeInputModel(),
                EditChatThemeViewModels = this.editChatThemeService.GetAllThemes(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExtractThemeData(string themeId)
        {
            GetEditChatThemeDataViewModel section = await this.editChatThemeService.GetThemeById(themeId);
            return new JsonResult(section);
        }

        [HttpPost]
        public async Task<IActionResult> EditChatTheme(EditChatThemeBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.editChatThemeService.EditChatTheme(model.EditChatThemeInput);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "EditChatTheme", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "EditChatTheme");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "EditChatTheme", model);
            }
        }
    }
}