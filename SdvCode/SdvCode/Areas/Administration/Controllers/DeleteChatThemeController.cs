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
    using SdvCode.Areas.Administration.Services.DeleteChatTheme;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DeleteChatThemeController : Controller
    {
        private readonly IDeleteChatThemeService removeChatThemeService;

        public DeleteChatThemeController(IDeleteChatThemeService removeChatThemeService)
        {
            this.removeChatThemeService = removeChatThemeService;
        }

        public IActionResult Index()
        {
            var model = new DeleteChatThemeBaseViewModel
            {
                DeleteChatThemeInputModel = new DeleteChatThemeInputModel(),
                DeleteChatThemeView = this.removeChatThemeService.GetAllChatThemes(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExtractThemeData(int themeId)
        {
            GetThemeDataViewModel section = await this.removeChatThemeService.GetThemeById(themeId);
            return new JsonResult(section);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChatTheme(DeleteChatThemeBaseViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.removeChatThemeService.DeleteChatTheme(model.DeleteChatThemeInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "DeleteChatTheme", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "DeleteChatTheme");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "DeleteChatTheme", model);
            }
        }
    }
}