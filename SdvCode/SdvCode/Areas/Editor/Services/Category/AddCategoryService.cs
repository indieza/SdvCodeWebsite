// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;

    public class AddCategoryService : IAddCategoryService
    {
        private readonly ApplicationDbContext db;

        public AddCategoryService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateCategory(string name, string description)
        {
            if (this.db.Categories.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return false;
            }

            var category = new Category
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                Description = description,
            };

            this.db.Categories.Add(category);
            await this.db.SaveChangesAsync();
            return true;
        }
    }
}