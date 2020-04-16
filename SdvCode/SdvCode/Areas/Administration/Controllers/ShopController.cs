// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Administration.Services.Shop;
    using SdvCode.Areas.SdvShop.ViewModels.Category.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Product.InputModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class ShopController : Controller
    {
        private readonly IShopDbUsageService shopDbUsageService;

        public ShopController(IShopDbUsageService shopDbUsageService)
        {
            this.shopDbUsageService = shopDbUsageService;
        }

        public IActionResult AddNewProductCategory()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProductCategory(ProductCategoryInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<string, string> tuple =
                    await this.shopDbUsageService.AddCategory(model.Title, model.SanitizedDescription);

                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddNewProductCategory", "Shop");
        }

        public IActionResult AddNewProduct()
        {
            ProductIndexModel model = new ProductIndexModel
            {
                ProductInputModel = new ProductInputModel(),
                ProductCategories = this.shopDbUsageService.ExtractAllCategories(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(ProductIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                Tuple<string, string> tuple =
                    await this.shopDbUsageService.AddProduct(model.ProductInputModel);

                this.TempData[tuple.Item1] = tuple.Item2;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("AddNewProduct", "Shop");
        }
    }
}