// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels;
    using X.PagedList;

    public class BlogComponentService : IBlogComponentService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogComponentService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<RecentPostsViewModel>> ExtractRecentPosts(HttpContext httpContext)
        {
            var currentUser = await this.userManager.GetUserAsync(httpContext.User);
            List<Post> posts = new List<Post>();
            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = this.db.Posts
                    .Where(x => x.PostStatus == PostStatus.Banned || x.PostStatus == PostStatus.Pending || x.PostStatus == PostStatus.Approved)
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }
            else
            {
                posts = this.db.Posts
                    .Where(x => x.PostStatus != PostStatus.Banned && x.PostStatus != PostStatus.Pending)
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }

            var recentPosts = new List<RecentPostsViewModel>();

            foreach (var post in posts.Take(20))
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                recentPosts.Add(new RecentPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                    PostStatus = post.PostStatus,
                });
            }

            return await recentPosts.ToListAsync();
        }

        public async Task<List<TopCategoriesViewModel>> ExtractTopCategories()
        {
            var categories = this.db.Categories.ToList();
            var topCategories = new List<TopCategoriesViewModel>();

            foreach (var category in categories.Take(10))
            {
                topCategories.Add(new TopCategoriesViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    PostsCount = this.db.Posts.Count(x => x.CategoryId == category.Id),
                });
            }

            return await topCategories.OrderByDescending(x => x.PostsCount).ToListAsync();
        }

        public async Task<List<TopPostsViewModel>> ExtractTopPosts(HttpContext httpContext)
        {
            var currentUser = await this.userManager.GetUserAsync(httpContext.User);
            List<Post> posts = new List<Post>();

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = this.db.Posts
                    .Where(x => x.PostStatus == PostStatus.Banned || x.PostStatus == PostStatus.Pending || x.PostStatus == PostStatus.Approved)
                    .OrderByDescending(x => x.Comments.Count + x.Likes)
                    .ToList();
            }
            else
            {
                posts = this.db.Posts
                    .Where(x => x.PostStatus != PostStatus.Banned && x.PostStatus != PostStatus.Pending)
                    .OrderByDescending(x => x.Comments.Count + x.Likes)
                    .ToList();
            }

            var topPosts = new List<TopPostsViewModel>();

            foreach (var post in posts.Take(10))
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                topPosts.Add(new TopPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                    PostStatus = post.PostStatus,
                });
            }

            return await topPosts.ToListAsync();
        }

        public async Task<List<TopTagsViewModel>> ExtractTopTags()
        {
            var tags = this.db.Tags.ToList();
            var topTags = new List<TopTagsViewModel>();

            foreach (var tag in tags.Take(10))
            {
                topTags.Add(new TopTagsViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Count = this.db.PostsTags.Count(x => x.TagId == tag.Id),
                });
            }

            return await topTags.OrderByDescending(x => x.Count).ToListAsync();
        }
    }
}