// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Shop.ShopDbUsage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Category.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Category.ViewModels;
    using SdvCode.Areas.SdvShop.ViewModels.Product.InputModels;
    using SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels;

    public interface IShopDbUsageService
    {
        Task<Tuple<string, string>> AddCategory(string title, string description);

        ICollection<string> ExtractAllCategoriesNames();

        Task<Tuple<string, string>> AddProduct(ProductInputModel productInputModel);

        ICollection<string> ExtractAllProductsNames();

        Task<EditProductViewModel> GetProductByName(string productName);

        Task<Tuple<string, string>> EditProduct(EditProductInputModel inputModel);

        Task<EditProductCategoryViewModel> GetProductCategoryByName(string categoryName);

        Task<Tuple<string, string>> EditProductCategory(EditProductCategoryInputModel inputModel);
    }
}