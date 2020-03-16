// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Services.Blog;

    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [Authorize]
        [Route("/Blog/{title}/Like/{id}")]
        public async Task<IActionResult> LikePost(string id, string title)
        {
            bool isLiked = await this.postService.LikePost(id, title);

            if (isLiked)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyLikePost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Blog");
        }
    }
}