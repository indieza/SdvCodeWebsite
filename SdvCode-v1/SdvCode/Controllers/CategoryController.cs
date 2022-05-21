﻿// Copyright (c) SDV Code Project. All rights reserved.
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
    using SdvCode.ViewModels.Category.ViewModels.CategoryPage;

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

        /// <summary>
        /// This function will get all categories with there related Blog Posts.
        /// </summary>
        /// <param name="id">Target Category ID.</param>
        /// <param name="page">Target page for displayed items.</param>
        /// <returns>Return a View with a View Model with all Categories and there Blog Posts.</returns>
        [HttpGet]
        [Authorize]
        [Route("Blog/Category/{id}/{page?}")]
        public async Task<IActionResult> Index(string id, int? page)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var pageNumber = page ?? 1;
            var posts = await this.categoryService.ExtractPostsByCategoryId(id, currentUser);

            CategoryPageViewModel model = new CategoryPageViewModel
            {
                Category = await this.categoryService.ExtractCategoryById(id),
                Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage),
            };

            return this.View(model);
        }
    }
}