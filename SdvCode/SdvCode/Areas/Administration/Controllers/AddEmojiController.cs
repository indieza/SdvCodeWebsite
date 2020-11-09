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
    using SdvCode.Areas.Administration.Services.AddEmoji;
    using SdvCode.Areas.Administration.ViewModels.EmojiViewModels.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddEmojiController : Controller
    {
        private readonly IAddEmojiService addEmojiService;

        public AddEmojiController(IAddEmojiService addEmojiService)
        {
            this.addEmojiService = addEmojiService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmoji(AddEmojiInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.addEmojiService.AddEmoji(model);
                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "AddEmoji", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "AddEmoji");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "AddEmoji", model);
            }
        }
    }
}