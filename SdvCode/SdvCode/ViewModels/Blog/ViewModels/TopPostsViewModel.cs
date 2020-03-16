// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.User;

    public class TopPostsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}