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

    using SdvCode.ApplicationAttributes.ActionAttributes;
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

        public EditCategoryController(IEditCategoryService editCategoryService)
        {
            this.editCategoryService = editCategoryService;
        }

        [Route("Editor/EditCategory/{id?}")]
        [HttpGet]
        [IsUserBlocked("Index", "Profile")]
        public async Task<IActionResult> Index(string id)
        {
            EditCategoryInputModel model = await this.editCategoryService.ExtractCategoryById(id);
            return this.View(model);
        }

        [Route("Editor/EditCategory/{id?}")]
        [HttpPost]
        [IsUserBlocked("Index", "Profile")]
        public async Task<IActionResult> Index(EditCategoryInputModel model)
        {
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