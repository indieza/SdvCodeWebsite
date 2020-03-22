// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Users.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Models.Enums;

    public class ManageAccountInputModel
    {
        [Phone]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [PersonalData]
        [Display(Name = "Country")]
        [MaxLength(20)]
        public string Country { get; set; }

        [PersonalData]
        [Display(Name = "City")]
        [MaxLength(20)]
        public string City { get; set; }

        [PersonalData]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [PersonalData]
        [Display(Name = "Registered On")]
        public DateTime RegisteredOn { get; set; }

        [PersonalData]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [PersonalData]
        [Display(Name = "Country Code")]
        public CountryCode CountryCode { get; set; }

        [PersonalData]
        [Display(Name = "About Me")]
        [MaxLength(600)]
        public string AboutMe { get; set; }

        [PersonalData]
        [Display(Name = "First Name")]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Last Name")]
        [MaxLength(15)]
        public string LastName { get; set; }

        [PersonalData]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        [PersonalData]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        [PersonalData]
        [Display(Name = "GitHub Profile")]
        public string GitHubUrl { get; set; }

        [PersonalData]
        [Display(Name = "Stack-overflow Profile")]
        public string StackoverflowUrl { get; set; }

        [PersonalData]
        [Display(Name = "Facebook Profile")]
        public string FacebookUrl { get; set; }

        [PersonalData]
        [Display(Name = "LinkedIn Profile")]
        public string LinkedinUrl { get; set; }

        [PersonalData]
        [Display(Name = "Twitter Profile")]
        public string TwitterUrl { get; set; }

        [PersonalData]
        [Display(Name = "Instagram Profile")]
        public string InstagramUrl { get; set; }
    }
}