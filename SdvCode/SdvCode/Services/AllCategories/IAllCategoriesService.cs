// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.AllCategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.ViewModels.AllCategories.ViewModels;

    public interface IAllCategoriesService
    {
        ICollection<AllCategoriesViewModel> GetAllBlogCategories();
    }
}