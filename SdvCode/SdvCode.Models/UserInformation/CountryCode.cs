// <copyright file="CountryCode.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class CountryCode : BaseDataModel
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