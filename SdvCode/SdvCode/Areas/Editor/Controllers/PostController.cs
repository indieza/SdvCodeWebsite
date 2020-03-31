// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Editor.Services;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using SdvCode.Services.Post;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class PostController : Controller
    {
        private readonly IEditorPostService postService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly GlobalUserValidator userValidator;

        public PostController(
            IEditorPostService postService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            this.postService = postService;
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.userValidator = new GlobalUserValidator(this.userManager);
        }

        public async Task<IActionResult> ApprovePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isApproved = await this.postService.ApprovePost(id);
            if (isApproved)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyApprovedBlogPost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> UnbanPost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isBanned = await this.postService.UnbannPost(id);
            if (isBanned)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyUnannedBlogPost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> BanPost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isBanned = await this.postService.BannPost(id);
            if (isBanned)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyBannedBlogPost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }
    }
}