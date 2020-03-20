// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class EditCategoryService : IEditCategoryService
    {
        private readonly ApplicationDbContext db;

        public EditCategoryService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> EditCategory(EditCategoryInputModel model)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (category != null)
            {
                category.Name = model.Name;
                category.Description = model.SanitizedDescription;
                category.UpdatedOn = DateTime.UtcNow;
                this.db.Categories.Update(category);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<EditCategoryInputModel> ExtractCategoryById(string id)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return new EditCategoryInputModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }
    }
}