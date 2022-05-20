﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions.BlogActions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class LikePostUserAction : BaseBlogAction
    {
        public LikePostUserAction()
        {
            this.ActionType = UserActionType.LikePost;
        }

        //[Required]
        //[ForeignKey(nameof(Post))]
        //public string PostId { get; set; }

        //public Post Post { get; set; }
    }
}