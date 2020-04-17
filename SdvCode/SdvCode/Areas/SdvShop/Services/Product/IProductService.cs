// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.Services.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.SdvShop.ViewModels.Product.ViewModels;

    public interface IProductService
    {
        Task<List<ProductCardViewModel>> ExtractProductsByCategoryId(string categoryId, string sorting);

        Task<List<ProductCardViewModel>> ExtractAllProducts(string sorting);
    }
}