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
    using SdvCode.ViewModels.Post.ViewModels.PostPage;

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
        [Route("/Blog/Post/{postId}")]
        [UserBlocked("Index", "Profile")]
        [BlogRole("Index", "Blog")]
        public async Task<IActionResult> Index(string postId)
        {
            if (!await this.postService.IsPostExist(postId))
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            PostViewModel model = await this.postService.ExtractCurrentPost(postId, currentUser);
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("/Blog/Post/LikePost/{postId}")]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotLikeNotApprovedBlogPost)]
        public async Task<IActionResult> LikePost(string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.LikePost(postId, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { postId });
        }

        [HttpPost]
        [Authorize]
        [Route("/Blog/Post/UnlikePost/{postId}")]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotUnlikeNotApprovedBlogPost)]
        public async Task<IActionResult> UnlikePost(string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.UnlikePost(postId, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { postId });
        }

        [HttpPost]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotAddToFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> AddToFavorite(string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.AddToFavorite(currentUser, postId);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { postId });
        }

        [HttpPost]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostActions("Index", "Blog", null, ErrorMessages.CannotRemoveFromFavoriteNotApprovedBlogPost)]
        public async Task<IActionResult> RemoveFromFavorite(string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.postService.RemoveFromFavorite(currentUser, postId);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { postId });
        }
    }
}