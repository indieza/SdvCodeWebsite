// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.DataViewModels.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Users.ViewModels;

    public class PostLikeViewModel
    {
        public string UserId { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public string PostId { get; set; }

        public PostViewModel Post { get; set; }

        public bool IsLiked { get; set; }
    }
}