// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.PrivateChat.Services.CollectStickers;
    using SdvCode.Areas.PrivateChat.ViewModels.CollectStickers.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using X.PagedList;

    [Authorize]
    [Area(GlobalConstants.PrivateChatArea)]
    public class CollectStickersController : Controller
    {
        private readonly ICollectStickersService collectStickersService;
        private readonly UserManager<ApplicationUser> userManager;

        public CollectStickersController(
            ICollectStickersService collectStickersService,
            UserManager<ApplicationUser> userManager)
        {
            this.collectStickersService = collectStickersService;
            this.userManager = userManager;
        }

        [Route("PrivateChat/CollectStickers/{page?}")]
        public async Task<IActionResult> Index(int? page)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var pageNumber = page ?? 1;

            var model = new CollectStickersBaseModel
            {
                AllStickerTypes = this.collectStickersService
                    .GetAllStickers(currentUser)
                    .ToPagedList(pageNumber, GlobalConstants.CollectStickersOnPage),
            };

            return this.View(model);
        }

        [HttpPost]
        [Route("PrivateChat/CollectStickers/AddStickerToFavourite")]
        public async Task<IActionResult> AddStickerToFavourite(string stickerTypeId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (stickerTypeId == null || stickerTypeId == string.Empty)
            {
                return new JsonResult(new { isAdded = false });
            }

            bool result = await this.collectStickersService.AddStickerToFavourite(currentUser, stickerTypeId);

            return new JsonResult(new { isAdded = result });
        }

        [HttpPost]
        [Route("PrivateChat/CollectStickers/RemoveStickerFromFavourite")]
        public async Task<IActionResult> RemoveStickerFromFavourite(string stickerTypeId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (stickerTypeId == null || stickerTypeId == string.Empty)
            {
                return new JsonResult(new { isRemoved = false });
            }

            bool result = await this.collectStickersService
                .RemoveStickerFromFavourite(currentUser, stickerTypeId);

            return new JsonResult(new { isRemoved = result });
        }
    }
}