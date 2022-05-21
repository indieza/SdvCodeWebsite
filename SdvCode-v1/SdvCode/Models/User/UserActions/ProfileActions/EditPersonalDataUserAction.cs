// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User.UserActions.ProfileActions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.Enums;

    public class EditPersonalDataUserAction : BaseUserAction
    {
        public EditPersonalDataUserAction()
        {
            this.ActionType = UserActionType.EditPersonalData;
        }

        [Required]
        public string Content { get; set; }
    }
}