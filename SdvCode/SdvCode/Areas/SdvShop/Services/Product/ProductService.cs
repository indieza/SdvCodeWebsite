// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels;
    using SdvCode.Data;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext db;

        public ProductService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<List<ProductCardViewModel>> ExtractAllProducts()
        {
            var products = this.db.Products.ToList();
            var result = new List<ProductCardViewModel>();

            foreach (var item in products)
            {
                var image = await this.db.ProductImages.FirstOrDefaultAsync(x => x.ProductId == item.Id);
                result.Add(new ProductCardViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    ImageUrl = image.ImageUrl,
                });
            }

            return result;
        }

        public async Task<List<ProductCardViewModel>> ExtractProductsByCategoryId(string id)
        {
            var products = this.db.Products.Where(x => x.ProductCategoryId == id).ToList();
            var result = new List<ProductCardViewModel>();

            foreach (var item in products)
            {
                var image = await this.db.ProductImages.FirstOrDefaultAsync(x => x.ProductId == item.Id);
                result.Add(new ProductCardViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    ImageUrl = image.ImageUrl,
                });
            }

            return result;
        }
    }
}