// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;

    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext db;

        public BlogService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<string> ExtractAllCategoryNames()
        {
            return this.db.Categories.Select(x => x.Name).ToList();
        }

        public ICollection<string> ExtractAllTagNames()
        {
            return this.db.Tags.Select(x => x.Name).ToList();
        }
    }
}