// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.Dashboard
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserInRoleInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }
    }
}