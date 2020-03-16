// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.ViewModels.Blog.ViewModels;
    using X.PagedList;

    public class BlogComponentService : IBlogComponentService
    {
        private readonly ApplicationDbContext db;

        public BlogComponentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Task<List<RecentPostsViewModel>> ExtractRecentPosts()
        {
            var posts = this.db.Posts.ToList().OrderByDescending(x => x.UpdatedOn).Take(3);
            var recentPosts = new List<RecentPostsViewModel>();

            foreach (var post in posts)
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                recentPosts.Add(new RecentPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                });
            }

            return recentPosts.ToListAsync();
        }

        public Task<List<TopCategoriesViewModel>> ExtractTopCategories()
        {
            var categories = this.db.Categories.ToList();
            var topCategories = new List<TopCategoriesViewModel>();

            foreach (var category in categories)
            {
                topCategories.Add(new TopCategoriesViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    PostsCount = this.db.Posts.Count(x => x.CategoryId == category.Id),
                });
            }

            return topCategories.OrderByDescending(x => x.PostsCount).Take(10).ToListAsync();
        }

        public Task<List<TopPostsViewModel>> ExtractTopPosts()
        {
            var posts = this.db.Posts.ToList().OrderByDescending(x => x.Comments.Count + x.Likes).Take(3);
            var topPosts = new List<TopPostsViewModel>();

            foreach (var post in posts)
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                topPosts.Add(new TopPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                });
            }

            return topPosts.ToListAsync();
        }

        public Task<List<TopTagsViewModel>> ExtractTopTags()
        {
            var tags = this.db.Tags.ToList();
            var topTags = new List<TopTagsViewModel>();

            foreach (var tag in tags)
            {
                topTags.Add(new TopTagsViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Count = this.db.PostsTags.Count(x => x.TagId == tag.Id),
                });
            }

            return topTags.OrderByDescending(x => x.Count).Take(10).ToListAsync();
        }
    }
}