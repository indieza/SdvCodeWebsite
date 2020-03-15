// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.InputModels;
    using Twilio.Rest.Api.V2010.Account.Usage;

    public class BlogController : Controller
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult CreatePost()
        {
            var model = new CreatePostIndexModel
            {
                Categories = this.blogService.ExtractAllCategoryNames(),
                Tags = this.blogService.ExtractAllTagNames(),
                PostInputModel = new CreatePostInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
            }

            return this.View();
        }
    }
}