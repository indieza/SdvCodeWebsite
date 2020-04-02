// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Post;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public PostController(
            IPostService postService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.postService = postService;
            this.userManager = userManager;
            this.db = db;
        }

        [Authorize]
        [Route("/Blog/Post/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.postService.IsBlocked(currentUser);
            if (isBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.postService.IsInBlogRole(currentUser);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.postService.IsPostApproved(id, currentUser);
            if (!isApproved)
            {
                this.TempData["Error"] = ErrorMessages.NotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            PostViewModel model = await this.postService.ExtractCurrentPost(id, currentUser);
            return this.View(model);
        }

        [Authorize]
        [Route("/Blog/Post/Like/{id}")]
        public async Task<IActionResult> LikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.postService.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.postService.IsInBlogRole(currentUser);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.postService.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotLikeNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            var tuple = await this.postService.LikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [Authorize]
        [Route("/Blog/Post/unlike/{id}")]
        public async Task<IActionResult> UnlikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.postService.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.postService.IsInBlogRole(currentUser);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.postService.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotUnlikeNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            var tuple = await this.postService.UnlikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> AddToFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.postService.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.postService.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotAddToFavoriteNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            var tuple = await this.postService.AddToFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> RemoveFromFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.postService.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.postService.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotRemoveFromFavoriteNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            var tuple = await this.postService.RemoveFromFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }
    }
}