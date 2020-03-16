// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;

    public interface IBlogService
    {
        ICollection<string> ExtractAllCategoryNames();

        ICollection<string> ExtractAllTagNames();

        Task<bool> CreatePost(CreatePostIndexModel model, ApplicationUser user);

        ICollection<Post> ExtraxtAllPosts();

        ICollection<TopCategoriesViewModel> ExtractTopCategories();

        ICollection<TopTagsViewModel> ExtractTopTags();

        ICollection<TopPostsViewModel> ExtractTopPosts();

        ICollection<RecentPostsViewModel> ExtractRecentPosts();
    }
}