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
    using SdvCode.Areas.Administration.Services.DeleteEmojisByType;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmojisByType.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DeleteEmojisByTypeController : Controller
    {
        private readonly IDeleteEmojisByTypeService deleteEmojisByTypeService;

        public DeleteEmojisByTypeController(IDeleteEmojisByTypeService deleteEmojisByTypeService)
        {
            this.deleteEmojisByTypeService = deleteEmojisByTypeService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmojisByType(DeleteEmojisByTypeInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.TempData["Success"] =
                    await this.deleteEmojisByTypeService.DeleteEmojisByType(model.EmojiType);
                return this.RedirectToAction("Index", "DeleteEmojisByType", model);
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "DeleteEmojisByType", model);
            }
        }
    }
}