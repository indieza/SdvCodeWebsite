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
    using SdvCode.Areas.Administration.Services.AddChatStickerType;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickerType.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddChatStickerTypeController : Controller
    {
        private readonly IAddChatStickerTypeService addChatStickerTypeService;

        public AddChatStickerTypeController(IAddChatStickerTypeService addChatStickerTypeService)
        {
            this.addChatStickerTypeService = addChatStickerTypeService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewStickerType(AddChatStickerTypeInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result = await this.addChatStickerTypeService.AddNewStickerType(model);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "AddChatStickerType", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "AddChatStickerType");
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "AddChatStickerType", model);
        }
    }
}