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
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<Category> ExtractCategoryById(string id)
        {
            return await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Post>> ExtractPostsByCategoryId(string id, HttpContext httpContext)
        {
            var posts = await this.db.Posts.Where(x => x.CategoryId == id).ToListAsync();
            var user = await this.userManager.GetUserAsync(httpContext.User);

            foreach (var post in posts)
            {
                post.ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id);
                post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == user.Id && x.IsLiked == true);

                var usersIds = this.db.PostsLikes.Where(x => x.PostId == post.Id && x.IsLiked == true).Select(x => x.UserId).ToList();
                foreach (var userId in usersIds)
                {
                    post.Likers.Add(this.db.Users.FirstOrDefault(x => x.Id == userId));
                }
            }

            return posts;
        }
    }
}