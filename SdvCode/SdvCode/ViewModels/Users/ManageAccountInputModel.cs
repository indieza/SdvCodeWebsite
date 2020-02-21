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
        public string PhoneNumber { get; set; }

        [PersonalData]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [PersonalData]
        [Display(Name = "City")]
        public string City { get; set; }

        [PersonalData]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [PersonalData]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
    }
}