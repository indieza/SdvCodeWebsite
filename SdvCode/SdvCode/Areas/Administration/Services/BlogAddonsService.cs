// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;

    public class BlogAddonsService : IBlogAddonsService
    {
        private readonly ApplicationDbContext db;
        private readonly CreateCategoryGlobally createCategoryGlobally;

        public BlogAddonsService(ApplicationDbContext db)
        {
            this.db = db;
            this.createCategoryGlobally = new CreateCategoryGlobally(this.db);
        }

        public async Task<bool> CreateCategory(string name, string description)
        {
            return await this.createCategoryGlobally.CreateCategory(name, description);
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