// <copyright file="User.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.User
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Constraints;

    public class User : IdentityUser
    {
        public User()
        {
        }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string MiddleName { get; set; }

        [MaxLength(DataModelConstants.UserNamesMaxLength)]
        public string LastName { get; set; }
    }
}