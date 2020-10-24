// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.BlogAddons;
    using SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class BlogAddonsController : Controller
    {
        private readonly IBlogAddonsService addonsService;

        public BlogAddonsController(IBlogAddonsService addonsService)
        {
            this.addonsService = addonsService;
        }

        public IActionResult AddTag()
        {
            return this.View();
        }

        public IActionResult AddCategory()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCategory(AddCategoryInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var tuple = await this.addonsService
                    .CreateCategoryAdminArea(model.Name, model.Description = model.SanitizedDescription);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddCategory", "BlogAddons");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTag(AddTagInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var tuple = await this.addonsService.CreateTag(model.Name);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddTag", "BlogAddons");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTag(string name)
        {
            if (name != null)
            {
                var tuple = await this.addonsService.RemoveTag(name);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddTag", "BlogAddons");
        }
    }
}