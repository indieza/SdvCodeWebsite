﻿// Copyright (c) SDV Code Project. All rights reserved.
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
    using SdvCode.Areas.PrivateChat.Models;
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
        private readonly IPrivateChatService privateChatService;

        public PrivateChatController(
            UserManager<ApplicationUser> userManager,
            IPrivateChatService privateChatService)
        {
            this.userManager = userManager;
            this.privateChatService = privateChatService;
        }

        [Route("PrivateChat/With/{username?}/Group/{group?}")]
        public async Task<IActionResult> Index(string username, string group)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

            if (!isAvailableToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { username });
            }

            var model = new PrivateChatViewModel
            {
                FromUser = await this.userManager.GetUserAsync(this.HttpContext.User),
                ToUser = await this.userManager.FindByNameAsync(username),
                ChatMessages = await this.privateChatService.ExtractAllMessages(group),
                GroupName = group,
                Emojis = this.privateChatService.GetAllEmojis(),
                AllChatThemes = this.privateChatService.GetAllThemes(),
                ChatThemeViewModel = this.privateChatService.GetGroupTheme(group),
                AllStickers = this.privateChatService.GetAllStickers(currentUser),
                AllQuickChatReplies = this.privateChatService.GetAllQuickReplies(currentUser),
            };

            return this.View(model);
        }

        [HttpGet]
        [Route("PrivateChat/With/{username?}/Group/{group?}/LoadMoreMessages/{messagesSkipCount?}")]
        public async Task<IActionResult> LoadMoreMessages(string username, string group, int? messagesSkipCount)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

            if (!isAvailableToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { username });
            }

            if (messagesSkipCount == null)
            {
                messagesSkipCount = 0;
            }

            ICollection<LoadMoreMessagesViewModel> data =
                await this.privateChatService.LoadMoreMessages(group, (int)messagesSkipCount, currentUser);
            return new JsonResult(data);
        }

        [HttpPost]
        [Route("PrivateChat/With/{username?}/Group/{group?}/ChangeChatTheme")]
        public async Task ChangeChatTheme(string username, string group, string themeId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

            if (isAvailableToChat)
            {
                await this.privateChatService.ChangeChatTheme(username, group, themeId);
            }
        }

        [HttpPost]
        [Route("PrivateChat/With/{toUsername?}/Group/{group?}/SendFiles")]
        public async Task<IActionResult> SendFiles(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(toUsername, group, currentUser);

            if (!isAvailableToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { Username = toUsername });
            }

            var result = await this.privateChatService
                .SendMessageWitFilesToUser(files, group, toUsername, fromUsername, message);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("PrivateChat/With/{username?}/Group/{group?}/AddChatQuickReply")]
        public async Task<IActionResult> AddChatQuickReply(string username, string group, string quickReplyText)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

            if (!isAvailableToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { Username = username });
            }

            if (quickReplyText == string.Empty || quickReplyText == null)
            {
                this.TempData["Error"] = ErrorMessages.CannotAddEmptyQuickReply;
                return this.RedirectToAction("Index", "Profile", new { Username = username });
            }

            QuickChatReplyViewModel result =
                await this.privateChatService.AddQuickChatReply(currentUser, quickReplyText);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("PrivateChat/With/{username?}/Group/{group?}/RemoveChatQuickReply")]
        public async Task<IActionResult> RemoveChatQuickReply(string username, string group, string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isAvailableToChat = await this.privateChatService.IsUserAbleToChat(username, group, currentUser);

            if (!isAvailableToChat)
            {
                this.TempData["Error"] = ErrorMessages.NotAbleToChat;
                return this.RedirectToAction("Index", "Profile", new { Username = username });
            }

            if (id == string.Empty || id == null)
            {
                this.TempData["Error"] = ErrorMessages.ChatQuickReplyDoesNotExist;
                return this.RedirectToAction("Index", "Profile", new { Username = username });
            }

            Tuple<bool, string> result = await this.privateChatService.RemoveQuickChatReply(currentUser, id);

            return new JsonResult(result);
        }
    }
}