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

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GlobalPostsExtractor postExtractor;

        public CategoryService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.postExtractor = new GlobalPostsExtractor(this.db);
        }

        public async Task<Category> ExtractCategoryById(string id)
        {
            return await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByCategoryId(string id, ApplicationUser user)
        {
            var posts = await this.db.Posts.Where(x => x.CategoryId == id).ToListAsync();
            List<PostViewModel> postsModel = await this.postExtractor.ExtractPosts(user, posts);
            return postsModel;
        }
    }
}