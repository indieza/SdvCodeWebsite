using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.ViewModels.Users.ViewModels
{
    public class CityViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public StateViewModel State { get; set; }

        public string CountryId { get; set; }

        public CountryViewModel Country { get; set; }

        public ICollection<ZipCodeViewModel> ZipCodes { get; set; } = new HashSet<ZipCodeViewModel>();

        public ICollection<ApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ApplicationUserViewModel>();
    }
}