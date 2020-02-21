using Microsoft.AspNetCore.Identity;
using SdvCode.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Country { get; set; }

        public string City { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }
    }
}