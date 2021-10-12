using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.ViewModels.Users.ViewModels
{
    public class CountryViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCodeId { get; set; }

        public CountryCodeViewModel CountryCode { get; set; }

        public ICollection<StateViewModel> States { get; set; } = new HashSet<StateViewModel>();

        public ICollection<CityViewModel> Cities { get; set; } = new HashSet<CityViewModel>();

        public ICollection<ApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ApplicationUserViewModel>();
    }
}