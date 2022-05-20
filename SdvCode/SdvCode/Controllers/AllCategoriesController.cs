﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// DONE!
namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.Constraints;
    using SdvCode.Services.AllCategories;
    using SdvCode.ViewModels.AllCategories.ViewModels;

    using X.PagedList;

    [Authorize]
    public class AllCategoriesController : Controller
    {
        private readonly IAllCategoriesService allCategoriesService;

        public AllCategoriesController(IAllCategoriesService allCategoriesService)
        {
            this.allCategoriesService = allCategoriesService;
        }

        /// <summary>
        /// This function will return a list of all Categories with there TOP Blog Posts.
        /// </summary>
        /// <param name="page">Current page number.</param>
        /// <returns>Returns a view with a collection of all Categories with there Posts.</returns>
        [HttpGet]
        [Route("AllCategories/{page?}")]
        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            IEnumerable<AllCategoriesCategoryViewModel> model = this.allCategoriesService.GetAllBlogCategories();
            return this.View(model.ToPagedList(pageNumber, GlobalConstants.BlogCategoriesOnPage));
        }
    }
}