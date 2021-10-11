// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.AllCategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.ViewModels.AllCategories.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels;

    public class AllCategoriesService : IAllCategoriesService
    {
        private readonly ApplicationDbContext db;

        public AllCategoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<AllCategoriesViewModel> GetAllBlogCategories()
        {
            return this.db.Categories
                .Include(x => x.Posts)
                .ThenInclude(x => x.ApplicationUser)
                .OrderBy(x => x.Name)
                .Select(x => new AllCategoriesViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn,
                    ApprovedPostsCount = x.Posts.Count(y => y.PostStatus == PostStatus.Approved),
                    PendingPostsCount = x.Posts.Count(y => y.PostStatus == PostStatus.Pending),
                    BannedPostsCount = x.Posts.Count(y => y.PostStatus == PostStatus.Banned),
                    Posts = x.Posts
                        .Where(y => y.PostStatus == PostStatus.Approved)
                        .OrderBy(z => z.CreatedOn)
                        .Take(10)
                        .Select(m => new PostViewModel
                        {
                            Id = m.Id,
                            ImageUrl = m.ImageUrl,
                            Title = m.Title,
                            CreatedOn = m.CreatedOn,
                            UpdatedOn = m.UpdatedOn,
                            //ApplicationUser = m.ApplicationUser,
                        })
                        .ToList(),
                })
                .ToList();
        }
    }
}