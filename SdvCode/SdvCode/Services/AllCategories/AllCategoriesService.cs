// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.AllCategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.ViewModels.AllCategories.ViewModels;

    public class AllCategoriesService : IAllCategoriesService
    {
        private readonly ApplicationDbContext db;

        public AllCategoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<AllCategoriesViewModel> GetAllBlogCategories()
        {
            var categories = this.db.Categories.OrderBy(x => x.Name).ToList();
            var result = new List<AllCategoriesViewModel>();

            foreach (var category in categories)
            {
                var posts = this.db.Posts
                    .Where(x => x.CategoryId == category.Id && x.PostStatus == PostStatus.Approved)
                    .ToList()
                    .Take(10);
                var postsImages = new Dictionary<string, string>();

                foreach (var post in posts)
                {
                    postsImages.Add(post.ImageUrl, post.Title);
                }

                result.Add(new AllCategoriesViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedOn = category.CreatedOn,
                    UpdatedOn = category.UpdatedOn,
                    ApprovedPostsCount = this.db.Posts
                        .Count(x => x.CategoryId == category.Id && x.PostStatus == PostStatus.Approved),
                    PendingPostsCount = this.db.Posts
                        .Count(x => x.CategoryId == category.Id && x.PostStatus == PostStatus.Pending),
                    BannedPostsCount = this.db.Posts
                        .Count(x => x.CategoryId == category.Id && x.PostStatus == PostStatus.Banned),
                    PostsImages = postsImages,
                });
            }

            return result;
        }
    }
}