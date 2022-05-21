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
    using SdvCode.Areas.Administration.Services.AddEmojiWithSkin;
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddEmojiWithSkin.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddEmojiWithSkinController : Controller
    {
        private readonly IAddEmojiWithSkinService addEmojiWithSkinService;

        public AddEmojiWithSkinController(IAddEmojiWithSkinService addEmojiWithSkinService)
        {
            this.addEmojiWithSkinService = addEmojiWithSkinService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmojiWithSkin(AddEmojiWithSkinInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                string result = await this.addEmojiWithSkinService.AddEmojiWithSkin(model);
                this.TempData["Success"] = result;
                return this.RedirectToAction("Index", "AddEmojiWithSkin");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "AddEmojiWithSkin", model);
            }
        }
    }
}