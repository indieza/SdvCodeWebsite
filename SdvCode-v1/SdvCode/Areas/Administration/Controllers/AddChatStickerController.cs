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
    using SdvCode.Areas.Administration.Services.AddChatSticker;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddChatStickerController : Controller
    {
        private readonly IAddChatStickerService addChatStickerService;

        public AddChatStickerController(IAddChatStickerService addChatStickerService)
        {
            this.addChatStickerService = addChatStickerService;
        }

        public IActionResult Index()
        {
            var model = new AddChatStickerBaseModel
            {
                AddChatStickerInputModel = new AddChatStickerInputModel(),
                AddChatStickerViewModels = this.addChatStickerService.GetAllStickerTypes(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSticker(AddChatStickerBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.addChatStickerService.AddNewSticker(model.AddChatStickerInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "AddChatSticker", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "AddChatSticker");
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "AddChatSticker", model);
        }
    }
}