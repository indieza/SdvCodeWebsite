// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels.TopCategory;
    using SdvCode.ViewModels.Comment.ViewModels.RecentComment;
    using SdvCode.ViewModels.Post.ViewModels.RecentPost;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Tag.TopTag;

    public interface IBlogComponentService
    {
        List<TopCategoryViewModel> ExtractTopCategories();

        List<TopTagViewModel> ExtractTopTags();

        Task<List<TopPostViewModel>> ExtractTopPosts(ApplicationUser user);

        Task<List<RecentPostViewModel>> ExtractRecentPosts(ApplicationUser user);

        Task<ICollection<RecentCommentViewModel>> ExtractRecentComments(ApplicationUser currentUser);
    }
}