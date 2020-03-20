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
    using SdvCode.Models.User;
    using SdvCode.Services.Tag;
    using SdvCode.ViewModels.Tag;
    using X.PagedList;

    public class TagController : Controller
    {
        private readonly ITagService tagService;
        private readonly UserManager<ApplicationUser> userManager;

        public TagController(ITagService tagService, UserManager<ApplicationUser> userManager)
        {
            this.tagService = tagService;
            this.userManager = userManager;
        }

        [Authorize]
        [Route("Blog/Tag/{id}/{page?}")]
        public async Task<IActionResult> Index(string id, int? page)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var pageNumber = page ?? 1;
            var post = await this.tagService.ExtractPostsByTagId(id, user);

            TagViewModel model = new TagViewModel
            {
                Tag = await this.tagService.ExtractTagById(id),
                Posts = post.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage),
            };

            return this.View(model);
        }
    }
}