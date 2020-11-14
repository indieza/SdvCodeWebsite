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
    using SdvCode.Areas.Administration.Services.DeleteEmoji;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmoji.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DeleteEmojiController : Controller
    {
        private readonly IDeleteEmojiService deleteEmojiService;

        public DeleteEmojiController(IDeleteEmojiService deleteEmojiService)
        {
            this.deleteEmojiService = deleteEmojiService;
        }

        public IActionResult Index()
        {
            var model = new DeleteEmojiBaseModel
            {
                DeleteEmojiInputModel = new DeleteEmojiInputModel(),
                DeleteEmojiViewModels = this.deleteEmojiService.GetAllEmojis(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmojiData(string emojiId)
        {
            string url = await this.deleteEmojiService.GetEmojiUrl(emojiId);
            return new JsonResult(new { url = url });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmoji(DeleteEmojiBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.deleteEmojiService.DeleteEmoji(model.DeleteEmojiInputModel);
                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "DeleteEmoji", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "DeleteEmoji");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "DeleteEmoji", model);
            }
        }
    }
}