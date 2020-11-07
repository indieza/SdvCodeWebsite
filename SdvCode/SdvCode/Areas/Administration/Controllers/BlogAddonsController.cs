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
    using SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.BlogAddonsViewModels.ViewModels;
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
            var model = new AddRemoveTagBaseModel
            {
                AddRemoveTagInputModel = new AddRemoveTagInputModel(),
                TagsNames = this.addonsService.GetAllTags(),
            };
            return this.View(model);
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
                return this.RedirectToAction("AddCategory", "BlogAddons", model);
            }

            return this.RedirectToAction("AddCategory", "BlogAddons");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTag(AddRemoveTagBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                var tuple = await this.addonsService.CreateTag(model.AddRemoveTagInputModel.Name);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("AddTag", "BlogAddons", model);
            }

            return this.RedirectToAction("AddTag", "BlogAddons");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTag(AddRemoveTagBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                var tuple = await this.addonsService.RemoveTag(model.AddRemoveTagInputModel.Name);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("AddTag", "BlogAddons", model);
            }

            return this.RedirectToAction("AddTag", "BlogAddons");
        }

        public IActionResult EditCategory()
        {
            var model = new EditCategoryBaseModel
            {
                EditCategoryInputModel = new EditCategoryInputModel(),
                EditCategoryViewModels = this.addonsService.GetAllCategories(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditExistingCategory(EditCategoryBaseModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.addonsService.EditExistingCategory(model.EditCategoryInputModel);
                this.TempData["Success"] = string.Format(
                    SuccessMessages.SuccessfullyEditCategory,
                    model.EditCategoryInputModel.Name.ToUpper());
                return this.RedirectToAction("EditCategory", "BlogAddons");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("EditCategory", "BlogAddons", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExtractCategoryData(string categoryId)
        {
            GetCategoryDataViewModel section = await this.addonsService.GetCategoryById(categoryId);
            return new JsonResult(section);
        }
    }
}