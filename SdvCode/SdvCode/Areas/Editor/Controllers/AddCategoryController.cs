// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Editor.Services;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Constraints;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class AddCategoryController : Controller
    {
        private readonly IAddCategoryService addCategoryService;

        public AddCategoryController(IAddCategoryService addCategoryService)
        {
            this.addCategoryService = addCategoryService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddCategoryInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool isAdded = await this.addCategoryService.CreateCategory(model.Name, model.Description = model.SanitizedDescription);

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

            return this.RedirectToAction("Index", "Blog");
        }
    }
}