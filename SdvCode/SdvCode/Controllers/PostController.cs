// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.ApplicationAttributes.ActionAttributes;
    using SdvCode.ApplicationAttributes.ActionAttributes.Blog;
    using SdvCode.ApplicationAttributes.ActionAttributes.Blog.Post;
    using SdvCode.Constraints;
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
        [UserBlocked("Index", "Profile")]
        [BlogRole("Index", "Blog")]
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

        [Authorize]
        [Route("/Blog/Post/LikePost/{id}")]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotLikeNotApprovedBlogPost)]
        public async Task<IActionResult> LikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.LikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [Authorize]
        [Route("/Blog/Post/UnlikePost/{id}")]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotUnlikeNotApprovedBlogPost)]
        public async Task<IActionResult> UnlikePost(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.UnlikePost(id, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotAddToFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> AddToFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.AddToFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }

        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotRemoveFromFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> RemoveFromFavorite(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.RemoveFromFavorite(currentUser, id);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { id });
        }
    }
}