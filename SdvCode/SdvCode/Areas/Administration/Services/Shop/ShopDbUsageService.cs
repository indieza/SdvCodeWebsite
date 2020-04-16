// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.SdvShop.ViewModels.Product.InputModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class ShopDbUsageService : IShopDbUsageService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public ShopDbUsageService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
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

        public async Task<Tuple<string, string>> AddProduct(ProductInputModel productInputModel)
        {
            if (this.db.Products.Any(x => x.Name.ToLower() == productInputModel.Name.ToLower()))
            {
                return Tuple.Create(
                    "Error",
                    string.Format(ErrorMessages.ProductAlreadyExist, productInputModel.Name.ToUpper()));
            }

            var targetCategory = await this.db.ProductCategories
                .FirstOrDefaultAsync(x => x.Title == productInputModel.ProductCategory);

            var product = new Product
            {
                Name = productInputModel.Name,
                Description = productInputModel.SanitaizedDescription,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ProductCategoryId = targetCategory.Id,
                Price = productInputModel.Price,
            };

            for (int i = 0; i < productInputModel.ProductImages.Count(); i++)
            {
                var imageName = string.Format(GlobalConstants.ProductImageName, product.Id, i);

                var imageUrl =
                    await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        productInputModel.ProductImages.ElementAt(i),
                        imageName);

                if (imageUrl != null)
                {
                    var image = new ProductImage
                    {
                        ImageUrl = imageUrl,
                        Name = imageName,
                        ProductId = product.Id,
                    };

                    product.ProductImages.Add(image);
                }
            }

            this.db.Products.Add(product);
            await this.db.SaveChangesAsync();
            return Tuple.Create(
                "Success",
                string.Format(SuccessMessages.SuccessfullyAddedProduct, product.Name.ToUpper()));
        }

        public ICollection<string> ExtractAllCategories()
        {
            return this.db.ProductCategories.Select(x => x.Title).ToList();
        }
    }
}