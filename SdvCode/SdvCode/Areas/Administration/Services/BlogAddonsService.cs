// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Blog;

    public class BlogAddonsService : IBlogAddonsService
    {
        private readonly ApplicationDbContext db;

        public BlogAddonsService(ApplicationDbContext db)
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

        public async Task<bool> CreateTag(string name)
        {
            if (this.db.Tags.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return false;
            }

            var tag = new Tag
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
            };

            this.db.Tags.Add(tag);
            await this.db.SaveChangesAsync();
            return true;
        }
    }
}