// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;

    public class BlogViewModel
    {
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<RecentPostsViewModel> RecentPosts { get; set; } = new HashSet<RecentPostsViewModel>();

        public ICollection<TopCategoriesViewModel> TopCategories { get; set; } = new HashSet<TopCategoriesViewModel>();

        public ICollection<TopTagsViewModel> TopTags { get; set; } = new HashSet<TopTagsViewModel>();

        public ICollection<TopPostsViewModel> TopPosts { get; set; } = new HashSet<TopPostsViewModel>();
    }
}