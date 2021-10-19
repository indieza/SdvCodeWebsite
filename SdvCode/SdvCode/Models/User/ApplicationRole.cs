// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Constraints;

    public class ApplicationRole : IdentityRole
    {
        [Required]
        public int RoleLevel { get; set; }

        [MaxLength(GlobalConstants.RoldeDescriptionMaxLength)]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}