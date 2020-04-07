// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class CategoryService : GlobalPostsExtractor, ICategoryService
    {
        private readonly ApplicationDbContext db;

        public CategoryService(ApplicationDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<Category> ExtractCategoryById(string id)
        {
            return await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByCategoryId(string id, ApplicationUser user)
        {
            var posts = await this.db.Posts.Where(x => x.CategoryId == id).ToListAsync();
            List<PostViewModel> postsModel = await this.ExtractPosts(user, posts);
            return postsModel;
        }
    }
}