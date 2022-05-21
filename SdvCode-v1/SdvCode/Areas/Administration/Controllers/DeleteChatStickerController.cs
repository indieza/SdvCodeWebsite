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
    using SdvCode.Areas.Administration.Services.DeleteChatSticker;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DeleteChatStickerController : Controller
    {
        private readonly IDeleteChatStickerService deleteChatStickerService;

        public DeleteChatStickerController(IDeleteChatStickerService deleteChatStickerService)
        {
            this.deleteChatStickerService = deleteChatStickerService;
        }

        public IActionResult Index()
        {
            var model = new DeleteChatStickerBaseModel
            {
                DeleteChatStickerInputModel = new DeleteChatStickerInputModel(),
                DeleteChatStickerViewModel = this.deleteChatStickerService.GetAllStickers(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatStickerData(string stickerId)
        {
            string url = await this.deleteChatStickerService.GetStickerUrl(stickerId);
            return new JsonResult(new { url = url });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChatSticker(DeleteChatStickerBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.deleteChatStickerService.DeleteChatSticker(model.DeleteChatStickerInputModel);
                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "DeleteChatSticker", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "DeleteChatSticker");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "DeleteChatSticker", model);
            }
        }
    }
}