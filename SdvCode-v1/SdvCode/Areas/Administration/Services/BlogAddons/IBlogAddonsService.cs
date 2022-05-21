﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.BlogAddons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.BlogAddons.ViewModels;
    using SdvCode.Areas.Editor.ViewModels;
    using SdvCode.Models.Blog;

    public interface IBlogAddonsService
    {
        Task<Tuple<string, string>> CreateCategoryAdminArea(string name, string description);

        Task<Tuple<string, string>> CreateTag(string name);

        Task<Tuple<string, string>> RemoveTag(string name);

        ICollection<string> GetAllTags();

        ICollection<EditCategoryViewModel> GetAllCategories();

        Task<GetCategoryDataViewModel> GetCategoryById(string categoryId);

        Task EditExistingCategory(EditCategoryInputModel model);
    }
}