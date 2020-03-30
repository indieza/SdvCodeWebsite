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
    using SdvCode.Areas.PrivateChat.Services;
    using SdvCode.Areas.PrivateChat.ViewModels.PrivateChat;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    [Authorize]
    [Area(GlobalConstants.PrivateChatArea)]
    public class PrivateChatController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly IPrivateChatService privateChatService;

        public PrivateChatController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db,
            IPrivateChatService privateChatService)
        {
            this.userManager = userManager;
            this.db = db;
            this.privateChatService = privateChatService;
        }

        [Route("PrivateChat/With/{username?}/Group/{group?}")]
        public async Task<IActionResult> Index(string username, string group)
        {
            bool isAbaleToChat = await this.privateChatService.IsUserAbleToChat(username, group, this.HttpContext);

            if (!isAbaleToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { username });
            }

            var model = new PrivateChatViewModel
            {
                FromUser = await this.userManager.GetUserAsync(this.HttpContext.User),
                ToUser = this.db.Users.FirstOrDefault(x => x.UserName == username),
                ChatMessages = await this.privateChatService.ExtractAllMessages(group),
                GroupName = group,
            };

            return this.View(model);
        }
    }
}