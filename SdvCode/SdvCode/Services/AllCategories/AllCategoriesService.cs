// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.AllCategories
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.ViewModels.AllCategories.ViewModels;

    public class AllCategoriesService : IAllCategoriesService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AllCategoriesService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public ICollection<AllCategoriesCategoryViewModel> GetAllBlogCategories()
        {
            var categories = this.db.Categories
                .Include(x => x.Posts)
                .ThenInclude(x => x.ApplicationUser)
                .OrderBy(x => x.Name)
                .ToList();

            var model = this.mapper.Map<List<AllCategoriesCategoryViewModel>>(categories);
            return model;
        }
    }
}