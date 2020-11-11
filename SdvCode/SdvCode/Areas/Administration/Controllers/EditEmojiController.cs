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
    using SdvCode.Areas.Administration.Services.EditEmoji;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class EditEmojiController : Controller
    {
        private readonly IEditEmojiService editEmojiService;

        public EditEmojiController(IEditEmojiService editEmojiService)
        {
            this.editEmojiService = editEmojiService;
        }

        public IActionResult Index()
        {
            var model = new EditEmojiBaseModel
            {
                EditEmojiInputModel = new EditEmojiInputModel(),
                EditEmojiViewModel = this.editEmojiService.GetAllEmojis(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmojiData(string emojiId)
        {
            GetEditEmojiDataViewModel section = await this.editEmojiService.GetEmojiById(emojiId);
            return new JsonResult(section);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmoji(EditEmojiBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result = await this.editEmojiService.EditEmoji(model.EditEmojiInputModel);
                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "EditEmoji", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "EditEmoji");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "EditEmoji", model);
            }
        }
    }
}