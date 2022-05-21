// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Product.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditProductIndexModel
    {
        public ICollection<string> ProductCategories { get; set; } = new HashSet<string>();

        public ICollection<string> AllProductsNames { get; set; } = new HashSet<string>();

        public EditProductInputModel InputModel { get; set; }
    }
}