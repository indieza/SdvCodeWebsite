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
    using SdvCode.Constraints;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Post;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IPostService postService, UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.userManager = userManager;
        }

        [Authorize]
        [Route("/Blog/Post/{id}")]
        public IActionResult Index(string id)
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;

            if (user.IsBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            PostViewModel model = this.postService.ExtractCurrentPost(id, user);
            return this.View(model);
        }

        [Authorize]
        [Route("/Blog/Post/Like/{id}")]
        public async Task<IActionResult> LikePost(string id)
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;

            if (user.IsBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isLiked = await this.postService.LikePost(id, user);

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
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;

            if (user.IsBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isUnliked = await this.postService.UnlikePost(id, user);

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
    }
}