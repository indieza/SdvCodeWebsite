// <copyright file="ZipCode.cs" company="SDV Code Data Models">
// Copyright (c) SDV Code Data Models. All rights reserved.
// </copyright>

namespace SdvCode.Models.UserInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SdvCode.Constraints;
    using SdvCode.Models.User;

    public class ZipCode : BaseData
    {
        public ZipCode()
        {
        }

        [Required]
        [MaxLength(DataModelConstants.ZipCodeMaxLength)]
        public string Code { get; set; }

        [ForeignKey(nameof(City))]
        public string? CityId { get; set; }

        public City City { get; set; }

        public ICollection<User> ApplicationUsers { get; set; } = new HashSet<User>();
    }
}