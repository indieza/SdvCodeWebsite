// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.AllCategories.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.DataViewModels.Blog;

    public class AllCategoriesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Description { get; set; }

        public int BannedPostsCount { get; set; }

        public int PendingPostsCount { get; set; }

        public int ApprovedPostsCount { get; set; }

        public ICollection<PostViewModel> Posts { get; set; } = new HashSet<PostViewModel>();
    }
}