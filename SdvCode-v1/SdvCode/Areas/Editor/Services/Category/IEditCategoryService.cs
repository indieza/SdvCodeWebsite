﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Services;

    public interface IEditCategoryService
    {
        Task<EditCategoryInputModel> ExtractCategoryById(string id);

        Task<bool> EditCategory(EditCategoryInputModel model);
    }
}