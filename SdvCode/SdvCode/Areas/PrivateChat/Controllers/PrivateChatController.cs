// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using SdvCode.Areas.PrivateChat.Services;
    using SdvCode.Areas.PrivateChat.Services.PrivateChat;
    using SdvCode.Areas.PrivateChat.ViewModels.PrivateChat;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
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
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAbaleToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

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
                Emojis = this.privateChatService.GetAllEmojis(),
                AllChatThemes = this.privateChatService.GetAllThemes(),
                ChatThemeViewModel = this.privateChatService.GetGroupTheme(group),
            };

            return this.View(model);
        }

        [HttpPost]
        [Route("PrivateChat/With/{username?}/Group/{group?}/ChangeChatTheme")]
        public async Task ChangeChatTheme(string username, string group, string themeId)
        {
            await this.privateChatService.ChangeChatTheme(username, group, themeId);
        }

        [HttpPost]
        [Route("PrivateChat/With/{toUsername?}/Group/{group?}/SendFiles")]
        public async Task SendFiles(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message)
        {
            string resultMessage = await this.privateChatService
                .SendMessageWitFilesToUser(files, group, toUsername, fromUsername, message);
            await this.privateChatService.ReceiveNewMessage(fromUsername, resultMessage, group);
        }
    }
}