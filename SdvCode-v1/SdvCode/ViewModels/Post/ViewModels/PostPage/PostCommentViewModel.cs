﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Post.ViewModels.PostPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class PostCommentViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public CommentStatus CommentStatus { get; set; }

        public string ApplicationUserId { get; set; }

        public PostApplicationUserViewModel ApplicationUser { get; set; }

        public string PostId { get; set; }

        public string ParentCommentId { get; set; }

        public PostCommentViewModel ParentComment { get; set; }
    }
}