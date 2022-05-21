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
    using SdvCode.Areas.Administration.Services.AllChatStickers;
    using SdvCode.Areas.Administration.ViewModels.AllChatStickers.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class AllChatStickersController : Controller
    {
        private readonly IAllChatStickersService allChatStickersService;

        public AllChatStickersController(IAllChatStickersService allChatStickersService)
        {
            this.allChatStickersService = allChatStickersService;
        }

        public IActionResult Index()
        {
            var model = new List<AllChatStickersViewModel>(this.allChatStickersService.GetAllChatStickers());

            return this.View(model);
        }
    }
}