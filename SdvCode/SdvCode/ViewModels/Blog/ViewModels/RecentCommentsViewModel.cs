// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Blog.ViewModels
{
    using System;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class RecentCommentsViewModel
    {
        public ApplicationUser User { get; set; }

        public string ShortContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public CommentStatus CommentStatus { get; set; }

        public string PostId { get; set; }
    }
}