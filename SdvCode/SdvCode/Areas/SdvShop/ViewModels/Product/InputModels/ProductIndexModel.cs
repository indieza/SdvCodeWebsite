// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Product.InputModels
{
    using System.Collections;
    using System.Collections.Generic;

    public class ProductIndexModel
    {
        public ProductInputModel ProductInputModel { get; set; }

        public ICollection<string> ProductCategories { get; set; }
    }
}