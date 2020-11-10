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
    using Newtonsoft.Json;
    using SdvCode.Areas.Administration.Services.EditEmojiPosition;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPositionViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPositionViewModels.ViewModels;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class EditEmojiPositionController : Controller
    {
        private readonly IEditEmojiPositionService editEmojiPositionService;

        public EditEmojiPositionController(IEditEmojiPositionService editEmojiPositionService)
        {
            this.editEmojiPositionService = editEmojiPositionService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult GetEmojisPosition(EmojiType emojiType)
        {
            ICollection<EditEmojiPositionViewModel> result =
                 this.editEmojiPositionService.GetAllEmojisByType(emojiType);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmojisPosition(string json)
        {
            var allEmojis = JsonConvert.DeserializeObject<EditEmojiPositionInputModel[]>(json);
            var count = await this.editEmojiPositionService.EditEmojisPosition(allEmojis);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyEditEmojisPosition, count);
            return this.Json(this.Url.Action("Index", "EditEmojiPosition"));
        }
    }
}