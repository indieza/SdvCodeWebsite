// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.SdvShop.ViewModels.Category.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditProductCategoryIndexModel
    {
        public ICollection<string> AllCategoriesNames { get; set; } = new HashSet<string>();

        public EditProductCategoryInputModel InputModel { get; set; }
    }
}