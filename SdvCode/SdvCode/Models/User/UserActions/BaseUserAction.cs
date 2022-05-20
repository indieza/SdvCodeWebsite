// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Models.Enums;
    using SdvCode.Models.User.UserActions.BlogActions;

    public abstract class BaseUserAction
    {
        public BaseUserAction()
        {
            this.ActionStatus = UserActionStatus.Unread;
            this.CreatedOn = DateTime.UtcNow;
            this.SystemMessage = $"This user action was registered on {this.CreatedOn.ToLocalTime():dd:MMMM:yyyy}.";
        }

        [Key]
        [Required]
        [ForeignKey(nameof(UserAction))]
        public string UserActionId { get; set; }

        public UserAction UserAction { get; set; }

        [Required]
        [ForeignKey(nameof(User.ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public UserActionStatus ActionStatus { get; set; }

        [Required]
        [EnumDataType(typeof(UserActionType))]
        public UserActionType ActionType { get; set; }

        [MaxLength(350)]
        public string SystemMessage { get; set; }

        [ForeignKey(nameof(BaseBlogAction))]
        public string? BaseBlogActionId { get; set; }

        public BaseBlogAction BaseBlogAction { get; set; }
    }
}