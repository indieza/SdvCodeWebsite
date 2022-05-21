// <copyright file="Role.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Constraints;

    public class Role : IdentityRole
    {
        public Role()
        {
        }

        [Required]
        [Range(DataModelConstants.RoleMinLevel, DataModelConstants.RoleMaxLevel)]
        public int RoleLevel { get; set; }

        [MaxLength(DataModelConstants.RoleDescriptonMaxLength)]
        public string Description { get; set; }

        public virtual ICollection<UserRole> UsersRoles { get; set; } = new HashSet<UserRole>();
    }
}