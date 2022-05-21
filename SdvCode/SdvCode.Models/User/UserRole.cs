// <copyright file="UserRole.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.User
{
    using Microsoft.AspNetCore.Identity;

    public class UserRole : IdentityUserRole<string>
    {
        public UserRole()
        {
        }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}