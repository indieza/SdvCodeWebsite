// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Category.ViewModels.CategoryPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;

    public class CategoryPageViewModel
    {
        public CategoryViewModel Category { get; set; }

        public IEnumerable<BlogPostCardViewModel> Posts { get; set; } = new HashSet<BlogPostCardViewModel>();
    }
}