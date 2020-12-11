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
    using SdvCode.Areas.Administration.Services.EditChatStickerType;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class EditChatStickerTypeController : Controller
    {
        private readonly IEditChatStickerTypeService editChatStickerTypeService;

        public EditChatStickerTypeController(IEditChatStickerTypeService editChatStickerTypeService)
        {
            this.editChatStickerTypeService = editChatStickerTypeService;
        }

        public IActionResult Index()
        {
            var model = new EditChatStickerTypeBaseModel
            {
                EditChatStickerTypeInputModel = new EditChatStickerTypeInputModel(),
                EditChatStickerTypeViewModels = this.editChatStickerTypeService.GetAllChatStickerTypes(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetStickerTypeData(string stickerTypeId)
        {
            GetEditChatStickerTypeDataViewModel section =
                await this.editChatStickerTypeService.GetStickerTypeById(stickerTypeId);
            return new JsonResult(section);
        }

        [HttpPost]
        public async Task<IActionResult> EditStickerType(EditChatStickerTypeBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.editChatStickerTypeService.EditStickerType(model.EditChatStickerTypeInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "EditChatStickerType", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "EditChatStickerType");
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "EditChatStickerType", model);
        }
    }
}