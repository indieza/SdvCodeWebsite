// <copyright file="City.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class City : BaseData
    {
        public City()
        {
        }

        [Required]
        [MaxLength(DataModelConstants.CityNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(State))]
        public string? StateId { get; set; }

        public State State { get; set; }

        [ForeignKey(nameof(Country))]
        public string? CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<ZipCode> ZipCodes { get; set; } = new HashSet<ZipCode>();

        public ICollection<User> ApplicationUsers { get; set; } = new HashSet<User>();
    }
}