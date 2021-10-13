// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class CategoryService : GlobalPostsExtractor, ICategoryService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
            : base(db)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<Category> ExtractCategoryById(string id)
        {
            return await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByCategoryId(string id, ApplicationUser user)
        {
            var posts = this.db.Posts.Where(x => x.CategoryId == id).ToList();

            if (user != null &&
                (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                posts = posts
                    .Where(x => x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending ||
                    x.PostStatus == PostStatus.Approved)
                    .ToList();
            }
            else
            {
                if (user != null)
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == user.Id)
                        .ToList();
                }
                else
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .ToList();
                }
            }

            List<PostViewModel> postsModel = await this.ExtractPosts(user, posts);
            return postsModel;
        }
    }
}