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
    using SdvCode.Areas.Administration.Services.DeleteChatStickerType;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DeleteChatStickerTypeController : Controller
    {
        private readonly IDeleteChatStickerTypeService deleteChatStickerTypeService;

        public DeleteChatStickerTypeController(IDeleteChatStickerTypeService deleteChatStickerTypeService)
        {
            this.deleteChatStickerTypeService = deleteChatStickerTypeService;
        }

        public IActionResult Index()
        {
            var model = new DeleteChatStickerTypeBaseModel
            {
                DeleteChatStickerTypeInputModel = new DeleteChatStickerTypeInputModel(),
                DeleteChatStickerTypeViewModel = this.deleteChatStickerTypeService.GetAllStickersTypes(),
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult GetChatStickerTypeData(string stickerTypeId)
        {
            List<string> urls = this.deleteChatStickerTypeService.GetStickersUrls(stickerTypeId);
            return new JsonResult(new { urls = new List<string>(urls) });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChatStickerType(DeleteChatStickerTypeBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.deleteChatStickerTypeService
                    .DeleteChatStickerType(model.DeleteChatStickerTypeInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "DeleteChatStickerType", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "DeleteChatStickerType");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "DeleteChatStickerType", model);
            }
        }
    }
}