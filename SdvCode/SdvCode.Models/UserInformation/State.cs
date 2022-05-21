// <copyright file="State.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class State : BaseData
    {
        public State()
        {
        }

        [Required]
        [MaxLength(DataModelConstants.StateNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public string? CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; } = new HashSet<City>();

        public ICollection<User> ApplicationUsers { get; set; } = new HashSet<User>();
    }
}