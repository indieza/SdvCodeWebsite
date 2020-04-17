// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Product.InputModels;

    public interface IShopDbUsageService
    {
        Task<Tuple<string, string>> AddCategory(string title, string description);

        ICollection<string> ExtractAllCategories();

        Task<Tuple<string, string>> AddProduct(ProductInputModel productInputModel);
    }
}