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
    using SdvCode.Areas.Administration.Services.EditChatSticker;
    using SdvCode.Areas.Administration.ViewModels.EditChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatSticker.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class EditChatStickerController : Controller
    {
        private readonly IEditChatStickerService editChatStickerService;

        public EditChatStickerController(IEditChatStickerService editChatStickerService)
        {
            this.editChatStickerService = editChatStickerService;
        }

        public IActionResult Index()
        {
            var model = new EditChatStickerBaseModel
            {
                EditChatStickerInputModel = new EditChatStickerInputModel(),
                AllStikersTypes = this.editChatStickerService.GetAllStikersTypes(),
                EditChatStickerViewModels = this.editChatStickerService.GetAllStickers(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetStickerData(string stickerId)
        {
            GetEditChatStickerDataViewModel section =
                await this.editChatStickerService.GetStickerById(stickerId);
            return new JsonResult(section);
        }

        [HttpPost]
        public async Task<IActionResult> EditSticker(EditChatStickerBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.editChatStickerService.EditSticker(model.EditChatStickerInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "EditChatSticker", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "EditChatSticker");
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "EditChatSticker", model);
        }
    }
}