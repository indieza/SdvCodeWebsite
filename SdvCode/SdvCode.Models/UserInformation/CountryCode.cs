// <copyright file="CountryCode.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class CountryCode : BaseData
    {
        public CountryCode()
        {
        }

        [Required]
        [MaxLength(DataModelConstants.CountryCodeMaxLength)]
        public string Code { get; set; }

        public ICollection<Country> Coutries { get; set; } = new HashSet<Country>();

        public ICollection<User> ApplicationUsers { get; set; } = new HashSet<User>();
    }
}