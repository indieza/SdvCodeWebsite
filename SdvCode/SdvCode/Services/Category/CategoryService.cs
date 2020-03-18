// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext db;

        public CategoryService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Category> ExtractCategoryById(string id)
        {
            return await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Post>> ExtractPostsByCategoryId(string id, ApplicationUser user)
        {
            var posts = await this.db.Posts.Where(x => x.CategoryId == id).ToListAsync();

            foreach (var post in posts)
            {
                post.ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id);
                post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == user.Id && x.IsLiked == true);
            }

            return posts;
        }
    }
}