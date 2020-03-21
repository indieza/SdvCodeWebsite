// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services
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
        private readonly CreateCategoryGlobally createCategoryGlobally;

        public AddCategoryService(ApplicationDbContext db)
        {
            this.db = db;
            this.createCategoryGlobally = new CreateCategoryGlobally(this.db);
        }

        public async Task<bool> CreateCategory(string name, string description)
        {
            return await this.createCategoryGlobally.CreateCategory(name, description);
        }
    }
}