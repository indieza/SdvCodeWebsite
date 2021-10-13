// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.UserPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Blog;

    public class UserPostsViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; } = new HashSet<PostViewModel>();

        public string Action { get; set; }

        public string Username { get; set; }
    }
}