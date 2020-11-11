// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DbUsage.DeleteUsersImages
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteImagesByUsernameInputModel
    {
        [Required]
        public string Username { get; set; }
    }
}