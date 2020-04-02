// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Editor.Services;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Models.User;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class EditCategoryController : Controller
    {
        private readonly IEditCategoryService editCategoryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly GlobalUserValidator userValidator;

        public EditCategoryController(
            IEditCategoryService editCategoryService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            this.editCategoryService = editCategoryService;
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.userValidator = new GlobalUserValidator(this.userManager);
        }

        [Route("Editor/EditCategory/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            EditCategoryInputModel model = await this.editCategoryService.ExtractCategoryById(id);
            return this.View(model);
        }

        [Route("Editor/EditCategory/{id?}")]
        [HttpPost]
        public async Task<IActionResult> Index(EditCategoryInputModel model)
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isEdited = await this.editCategoryService.EditCategory(model);
            if (isEdited == true)
            {
                this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyEditCategory, model.Name);
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Category", new { id = model.Id });
        }
    }
}