// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Post.ViewModels;

    public class CategoryViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Description { get; set; }

        // TODO
        public Category Category { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; } = new HashSet<PostViewModel>();
    }
}