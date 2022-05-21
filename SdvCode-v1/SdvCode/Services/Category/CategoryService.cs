// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Category.ViewModels.CategoryPage;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public CategoryService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<CategoryPageCategoryViewModel> ExtractCategoryById(string id)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            var model = this.mapper.Map<CategoryPageCategoryViewModel>(category);
            return model;
        }

        public async Task<ICollection<BlogPostCardViewModel>> ExtractPostsByCategoryId(string id, ApplicationUser user)
        {
            Expression<Func<Post, bool>> filterFunction;

            if (user != null &&
                (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                filterFunction = x => (x.PostStatus == PostStatus.Banned ||
                      x.PostStatus == PostStatus.Pending ||
                      x.PostStatus == PostStatus.Approved) && x.CategoryId == id;
            }
            else
            {
                if (user != null)
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved || x.ApplicationUserId == user.Id;
                }
                else
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved;
                }
            }

            var posts = this.db.Posts
                .Include(x => x.ApplicationUser)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .Include(x => x.FavouritePosts)
                .Include(x => x.PostLikes)
                .AsSplitQuery()
                .Where(filterFunction)
                .OrderByDescending(x => x.UpdatedOn)
                .ToList();

            var postsModel = this.mapper.Map<List<BlogPostCardViewModel>>(posts);
            return postsModel;
        }
    }
}