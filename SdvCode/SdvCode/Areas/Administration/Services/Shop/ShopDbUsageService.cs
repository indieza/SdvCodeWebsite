// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class ShopDbUsageService : IShopDbUsageService
    {
        private readonly ApplicationDbContext db;

        public ShopDbUsageService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Tuple<string, string>> AddCategory(string title, string description)
        {
            if (this.db.ProductCategories.Any(x => x.Title.ToLower() == title.ToLower()))
            {
                return Tuple.Create(
                    "Error",
                    string.Format(ErrorMessages.ProductCategoryAlreadyExist, title.ToUpper()));
            }

            var category = new ProductCategory
            {
                Title = title,
                Description = description,
            };

            this.db.ProductCategories.Add(category);
            await this.db.SaveChangesAsync();

            return Tuple.Create(
                "Success",
                string.Format(SuccessMessages.SuccessfullyAddedProductCategory, title.ToUpper()));
        }
    }
}