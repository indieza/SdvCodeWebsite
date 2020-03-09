using SdvCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.ViewModels.Profiles
{
    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }
    }
}