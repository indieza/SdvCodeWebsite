// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.UsersPenalties
{
    using System.ComponentModel.DataAnnotations;

    public class UsersPenaltiesInputModel
    {
        [Required]
        public string Username { get; set; }

        [MaxLength(200)]
        [Display(Name = "Reason To Be Blocked")]
        public string ReasonToBeBlocked { get; set; }
    }
}