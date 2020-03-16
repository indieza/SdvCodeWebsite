// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.ViewModels.Blog.ViewModels;

    public interface IBlogComponentService
    {
        Task<List<TopCategoriesViewModel>> ExtractTopCategories();

        Task<List<TopTagsViewModel>> ExtractTopTags();

        Task<List<TopPostsViewModel>> ExtractTopPosts();

        Task<List<RecentPostsViewModel>> ExtractRecentPosts();
    }
}