// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Category.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels.TopCategory;
    using SdvCode.ViewModels.Comment.ViewModels.RecentComment;
    using SdvCode.ViewModels.Post.ViewModels.RecentPost;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Tag.TopTag;

    public class BlogComponentViewModel
    {
        public string Search { get; set; }

        public ICollection<RecentPostViewModel> RecentPosts { get; set; } = new HashSet<RecentPostViewModel>();

        public ICollection<TopCategoryViewModel> TopCategories { get; set; } = new HashSet<TopCategoryViewModel>();

        public ICollection<TopTagViewModel> TopTags { get; set; } = new HashSet<TopTagViewModel>();

        public ICollection<TopPostViewModel> TopPosts { get; set; } = new HashSet<TopPostViewModel>();

        public ICollection<RecentCommentViewModel> RecentComments { get; set; } = new HashSet<RecentCommentViewModel>();
    }
}