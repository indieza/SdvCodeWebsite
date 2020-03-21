// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
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
        private readonly GlobalUserValidator userValidator;

        public PostController(IPostService postService, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.postService = postService;
            this.userManager = userManager;
            this.db = db;
            this.userValidator = new GlobalUserValidator(this.userManager, this.db);
        }

        [Authorize]
        [Route("/Blog/Post/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInBlogRole(this.HttpContext);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostApproved(id, this.HttpContext);
            if (!isApproved)
            {
                this.TempData["Error"] = ErrorMessages.NotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            PostViewModel model = await this.postService.ExtractCurrentPost(id, this.HttpContext);
            return this.View(model);
        }

        [Authorize]
        [Route("/Blog/Post/Like/{id}")]
        public async Task<IActionResult> LikePost(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInBlogRole(this.HttpContext);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotLikeNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isLiked = await this.postService.LikePost(id, this.HttpContext);

            if (isLiked)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyLikePost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }

        [Authorize]
        [Route("/Blog/Post/unlike/{id}")]
        public async Task<IActionResult> UnlikePost(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInBlogRole(this.HttpContext);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotUnlikeNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isUnliked = await this.postService.UnlikePost(id, this.HttpContext);

            if (isUnliked)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyUnlikePost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> AddToFavorite(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotAddToFavoriteNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isAdded = await this.postService.AddToFavorite(this.HttpContext, id);
            if (isAdded)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyAddedToFavorite;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }

        public async Task<IActionResult> RemoveFromFavorite(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostBlockedOrPending(id);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotRemoveFromFavoriteNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isRemoved = await this.postService.RemoveFromFavorite(this.HttpContext, id);
            if (isRemoved)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyRemoveFromFavorite;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id });
        }
    }
}