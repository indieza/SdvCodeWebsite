// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Editor.Services;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Models.User;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class AddCategoryController : Controller
    {
        private readonly IAddCategoryService addCategoryService;
        private readonly UserManager<ApplicationUser> userManager;

        public AddCategoryController(
            IAddCategoryService addCategoryService,
            UserManager<ApplicationUser> userManager)
        {
            this.addCategoryService = addCategoryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (currentUser.IsBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddCategoryInputModel model)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (currentUser.IsBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            if (this.ModelState.IsValid)
            {
                var tuple = await this.addCategoryService
                    .CreateCategory(model.Name, model.Description = model.SanitizedDescription);
                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Blog", model);
            }

            return this.RedirectToAction("Index", "Blog");
        }
    }
}