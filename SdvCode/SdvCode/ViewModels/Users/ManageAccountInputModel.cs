using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SdvCode.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.ViewModels.Users
{
    public class ManageAccountInputModel
    {
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
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [PersonalData]
        [Display(Name = "About Me")]
        [MaxLength(250)]
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
    }
}