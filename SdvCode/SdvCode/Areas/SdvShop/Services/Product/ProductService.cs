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

        public async Task<List<ProductCardViewModel>> ExtractAllProducts(string sorting)
        {
            var products = this.db.Products.ToList();
            var result = new List<ProductCardViewModel>();

            foreach (var item in products)
            {
                var imageUrl = await this.db.ProductImages
                    .Where(x => x.ProductId == item.Id).OrderBy(x => x.Name)
                    .Select(x => x.ImageUrl)
                    .FirstOrDefaultAsync();
                result.Add(new ProductCardViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    ImageUrl = imageUrl,
                    CreatedOn = item.CreatedOn,
                });
            }

            return this.SortProducts(result, sorting);
        }

        public async Task<ProductViewModel> ExtractProductById(string productId)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == productId);
            var images = this.db.ProductImages.Where(x => x.ProductId == productId).OrderBy(x => x.Name).ToList();
            var category = await this.db.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CreatedOn = product.CreatedOn,
                AvailableQuantity = product.AvailableQuantity,
                Description = product.Description,
                SpecificationsDescription = product.SpecificationsDescription,
                Price = product.Price,
                ProductImages = images,
                UpdatedOn = product.UpdatedOn,
                ProductCategory = category,
                ProductCategoryId = category.Id,
            };
        }

        public async Task<List<ProductCardViewModel>> ExtractProductsByCategoryId(string categoryId, string sorting)
        {
            var products = this.db.Products.Where(x => x.ProductCategoryId == categoryId).ToList();
            var result = new List<ProductCardViewModel>();

            foreach (var item in products)
            {
                var imageUrl = await this.db.ProductImages
                    .Where(x => x.ProductId == item.Id).OrderBy(x => x.Name)
                    .Select(x => x.ImageUrl)
                    .FirstOrDefaultAsync();
                result.Add(new ProductCardViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    ImageUrl = imageUrl,
                    CreatedOn = item.CreatedOn,
                });
            }

            return this.SortProducts(result, sorting);
        }

        private List<ProductCardViewModel> SortProducts(List<ProductCardViewModel> result, string sorting) =>
            sorting switch
            {
                "Newest" => result.OrderByDescending(x => x.CreatedOn).ToList(),
                "Oldest" => result.OrderBy(x => x.CreatedOn).ToList(),
                "N_ASC" => result.OrderBy(x => x.Name).ToList(),
                "N_DESC" => result.OrderByDescending(x => x.Name).ToList(),
                "P_ASC" => result.OrderBy(x => x.Price).ToList(),
                "P_DESC" => result.OrderByDescending(x => x.Price).ToList(),
                _ => result.OrderByDescending(x => x.CreatedOn).ToList(),
            };
    }
}