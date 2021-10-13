// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels;

    public interface IBlogComponentService
    {
        List<TopCategoriesViewModel> ExtractTopCategories();

        List<TopTagsViewModel> ExtractTopTags();

        Task<List<TopPostsViewModel>> ExtractTopPosts(ApplicationUser user);

        List<RecentPostsViewModel> ExtractRecentPosts(ApplicationUser user);

        Task<ICollection<RecentCommentsViewModel>> ExtractRecentComments(ApplicationUser currentUser);
    }
}