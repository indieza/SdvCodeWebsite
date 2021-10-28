// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AngleSharp.Common;

    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;

    public class UserAction
    {
        public UserAction()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ActionStatus = UserActionStatus.Unread;
            this.CreatedOn = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public UserActionStatus ActionStatus { get; set; }

        [Required]
        [EnumDataType(typeof(UserActionType))]
        public UserActionType ActionType { get; set; }

        //public string PersonUsername { get; set; }

        //public string FollowerUsername { get; set; }

        //public string ProfileImageUrl { get; set; }

        //public string CoverImageUrl { get; set; }

        //[ForeignKey(nameof(Post))]
        //public string PostId { get; set; }

        //public Post Post { get; set; }

        //[MaxLength(150)]
        //public string PostTitle { get; set; }

        //[MaxLength(350)]
        //public string PostContent { get; set; }
    }
}