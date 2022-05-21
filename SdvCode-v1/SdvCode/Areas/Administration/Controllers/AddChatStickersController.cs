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
    using SdvCode.Areas.Administration.Services.AddChatStickers;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AddChatStickersController : Controller
    {
        private readonly IAddChatStickersService addChatStickersService;

        public AddChatStickersController(IAddChatStickersService addChatStickersService)
        {
            this.addChatStickersService = addChatStickersService;
        }

        public IActionResult Index()
        {
            var model = new AddChatStickersBaseModel
            {
                AddChatStickersInputModel = new AddChatStickersInputModel(),
                AddChatStickersViewModels = this.addChatStickersService.GetAllStickersTypes(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddChatStickers(AddChatStickersBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<bool, string> result =
                    await this.addChatStickersService.AddChatStickers(model.AddChatStickersInputModel);

                if (!result.Item1)
                {
                    this.TempData["Error"] = result.Item2;
                    return this.RedirectToAction("Index", "AddChatStickers", model);
                }

                this.TempData["Success"] = result.Item2;
                return this.RedirectToAction("Index", "AddChatStickers");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "AddChatStickers", model);
            }
        }
    }
}