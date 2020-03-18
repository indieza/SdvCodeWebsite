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
    using SdvCode.Models.User;
    using SdvCode.Services.Category;
    using SdvCode.ViewModels.Category;

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [Authorize]
        [Route("Blog/Category/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            CategoryViewModel model = new CategoryViewModel
            {
                Category = await this.categoryService.ExtractCategoryById(id),
                Posts = await this.categoryService.ExtractPostsByCategoryId(id, user),
            };

            return this.View(model);
        }
    }
}