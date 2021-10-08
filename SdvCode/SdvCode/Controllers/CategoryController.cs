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
    using SdvCode.Services.Category;
    using SdvCode.ViewModels.Category;

    using X.PagedList;

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("Blog/Category/{id}/{page?}")]
        public async Task<IActionResult> Index(string id, int? page)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var pageNumber = page ?? 1;
            var posts = await this.categoryService.ExtractPostsByCategoryId(id, currentUser);

            CategoryViewModel model = new CategoryViewModel
            {
                Category = await this.categoryService.ExtractCategoryById(id),
                Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage),
            };

            return this.View(model);
        }
    }
}