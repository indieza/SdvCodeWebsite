// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Comment.ViewModels.RecentComment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class RecentCommentViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string ShortContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public CommentStatus CommentStatus { get; set; }

        public RecentCommentApplicationUserViewModel ApplicationUser { get; set; }

        public string PostId { get; set; }
    }
}