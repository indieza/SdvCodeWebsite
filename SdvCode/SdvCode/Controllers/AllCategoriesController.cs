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
    using SdvCode.Services.AllCategories;
    using SdvCode.ViewModels.AllCategories.ViewModels;

    [Authorize]
    public class AllCategoriesController : Controller
    {
        private readonly IAllCategoriesService allCategoriesService;

        public AllCategoriesController(IAllCategoriesService allCategoriesService)
        {
            this.allCategoriesService = allCategoriesService;
        }

        public IActionResult Index()
        {
            ICollection<AllCategoriesViewModel> model = this.allCategoriesService.GetAllBlogCategories();

            return this.View(model);
        }
    }
}