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

    using SdvCode.ApplicationAttributes.ActionAttributes;
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

        public PostController(
            IPostService postService,
            UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("/Blog/Post/{id}")]
        [IsUserBlocked("Index", "Profile")]
        [CanAccessBlog("Index", "Blog")]
        [CanAccessBlogPost("Index", "Blog", ErrorMessages.NotApprovedBlogPost)]
        public async Task<IActionResult> Index(string id)
        {
            if (!await this.postService.IsPostExist(id))
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            PostViewModel model = await this.postService.ExtractCurrentPost(id, currentUser);
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("/Blog/Post/Like/{id}")]
        [IsUserBlocked("Index", "Profile")]
        [CanAccessBlog("Index", "Blog")]
        [CanAccessBlogPost("Index", "Blog", ErrorMessages.CanLikeNotApprovedBlogPost)]
        public async Task<IActionResult> LikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.LikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [HttpPost]
        [Authorize]
        [Route("/Blog/Post/unlike/{id}")]
        [IsUserBlocked("Index", "Profile")]
        [CanAccessBlog("Index", "Blog")]
        [CanAccessBlogPost("Index", "Blog", ErrorMessages.CannotUnlikeNotApprovedBlogPost)]
        public async Task<IActionResult> UnlikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.UnlikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [HttpPost]
        [Authorize]
        [IsUserBlocked("Index", "Profile")]
        [CanAccessBlogPost("Index", "Blog", ErrorMessages.CannotAddToFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> AddToFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.AddToFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [HttpPost]
        [Authorize]
        [IsUserBlocked("Index", "Profile")]
        [CanAccessBlogPost("Index", "Blog", ErrorMessages.CannotRemoveFromFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> RemoveFromFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.RemoveFromFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }
    }
}