// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Category.ViewModels;
    using SdvCode.Data;
    using X.PagedList;

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ApplicationDbContext db;

        public ProductCategoryService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<ProductCategoryViewModel> ExtractAllCategories()
        {
            var result = new List<ProductCategoryViewModel>();

            foreach (var category in this.db.ProductCategories.OrderBy(x => x.Title).ToList())
            {
                result.Add(new ProductCategoryViewModel
                {
                    Id = category.Id,
                    Title = category.Title,
                    Description = category.Description,
                });
            }

            return result;
        }
    }
}