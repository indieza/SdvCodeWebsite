// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions.BlogActions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Models.Blog;

    public abstract class BaseBlogAction : BaseUserAction
    {
        [Required]
        [ForeignKey(nameof(BaseUserAction))]
        public string BaseUserActionId { get; set; }

        public BaseUserAction BaseUserAction { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public Post Post { get; set; }
    }
}