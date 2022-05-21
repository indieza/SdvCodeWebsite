// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class UnfollowUserAction : BaseUserAction
    {
        public UnfollowUserAction()
        {
            this.ActionType = UserActionType.Unfollow;
        }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UnfollowingApplicationUserId { get; set; }

        public ApplicationUser UnfollowingApplicationUser { get; set; }
    }
}