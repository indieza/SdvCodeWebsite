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
    using SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    public class BlogAddonsController : Controller
    {
        private readonly IBlogAddonsService addonsService;

        public BlogAddonsController(IBlogAddonsService addonsService)
        {
            this.addonsService = addonsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult AddTag()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult AddCategory()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> AddNewCategory(AddCategoryInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool isAdded = await this.addonsService.CreateCategory(model.Name, model.Description = model.SanitizedDescription);

                if (isAdded)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedCategory, model.Name);
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.CategoryAlreadyExist, model.Name);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddCategory", "BlogAddons");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> AddNewTag(AddTagInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool isAdded = await this.addonsService.CreateTag(model.Name);

                if (isAdded)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedTag, model.Name);
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.TagAlreadyExist, model.Name);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddTag", "BlogAddons");
        }
    }
}