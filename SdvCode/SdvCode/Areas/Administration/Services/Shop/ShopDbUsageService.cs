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
    using Microsoft.VisualBasic;
    using SdvCode.Areas.SdvShop.Models;
    using SdvCode.Areas.SdvShop.ViewModels.Product.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels;
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
                AvailableQuantity = productInputModel.AvailableQuantity,
                SpecificationsDescription = productInputModel.SanitaizedSpecifications,
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

        public async Task<Tuple<string, string>> EditProduct(EditProductInputModel inputModel)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == inputModel.Id);

            if (product != null)
            {
                var category = await this.db.ProductCategories
                    .FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);

                if (category != null)
                {
                    product.ProductCategoryId = category.Id;
                    product.Name = inputModel.Name;
                    product.Description = inputModel.SanitaizedDescription;
                    product.AvailableQuantity = inputModel.AvailableQuantity;
                    product.SpecificationsDescription = inputModel.SanitaizedSpecifications;
                    product.Price = inputModel.Price;
                    product.UpdatedOn = DateTime.UtcNow;

                    if (inputModel.ProductImages.Count != 0)
                    {
                        var images = this.db.ProductImages.Where(x => x.ProductId == inputModel.Id).ToList();

                        foreach (var image in images)
                        {
                            ApplicationCloudinary.DeleteImage(this.cloudinary, image.Name);
                        }

                        this.db.ProductImages.RemoveRange(images);
                        await this.db.SaveChangesAsync();

                        for (int i = 0; i < inputModel.ProductImages.Count(); i++)
                        {
                            var imageName = string.Format(GlobalConstants.ProductImageName, product.Id, i);

                            var imageUrl =
                                await ApplicationCloudinary.UploadImage(
                                    this.cloudinary,
                                    inputModel.ProductImages.ElementAt(i),
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
                    }

                    this.db.Products.Update(product);
                    await this.db.SaveChangesAsync();
                    return Tuple.Create("Success", SuccessMessages.SuccessfullyEditedProduct);
                }

                return Tuple.Create("Error", ErrorMessages.InvalidInputModel);
            }

            return Tuple.Create("Error", ErrorMessages.InvalidInputModel);
        }

        public ICollection<string> ExtractAllCategoriesNames()
        {
            return this.db.ProductCategories.Select(x => x.Title).ToList();
        }

        public ICollection<string> ExtractAllProductsNames()
        {
            return this.db.Products.Select(x => x.Name).ToList();
        }

        public async Task<EditProductViewModel> GeProductByName(string productName)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Name == productName);
            var category = await this.db.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);

            return new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SpecificationsDescription = product.SpecificationsDescription,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity,
                ProductCategory = category.Title,
            };
        }
    }
}