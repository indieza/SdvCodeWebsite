// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Category
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Category.ViewModels.CategoryPage;

    public interface ICategoryService
    {
        Task<CategoryViewModel> ExtractCategoryById(string id);

        Task<ICollection<BlogPostCardViewModel>> ExtractPostsByCategoryId(string id, ApplicationUser user);
    }
}