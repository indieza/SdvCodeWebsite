// <copyright file="Country.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class Country : BaseData
    {
        public Country()
        {
        }

        [Required]
        [MaxLength(DataModelConstants.CountryNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public string? CountryCodeId { get; set; }

        public CountryCode CountryCode { get; set; }

        public ICollection<State> States { get; set; } = new HashSet<State>();

        public ICollection<City> Cities { get; set; } = new HashSet<City>();

        public ICollection<User> ApplicationUsers { get; set; } = new HashSet<User>();
    }
}