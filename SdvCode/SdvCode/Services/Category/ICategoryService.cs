// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public interface ICategoryService
    {
        Task<Category> ExtractCategoryById(string id);

        Task<ICollection<PostViewModel>> ExtractPostsByCategoryId(string id, HttpContext httpContext);
    }
}