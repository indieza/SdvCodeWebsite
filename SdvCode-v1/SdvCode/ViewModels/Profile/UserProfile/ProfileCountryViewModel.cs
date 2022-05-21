// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileCountryViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCodeId { get; set; }

        public ProfileCountryCodeViewModel CountryCode { get; set; }

        public ICollection<ProfileStateViewModel> States { get; set; } = new HashSet<ProfileStateViewModel>();

        public ICollection<ProfileCityViewModel> Cities { get; set; } = new HashSet<ProfileCityViewModel>();

        public ICollection<ProfileApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ProfileApplicationUserViewModel>();
    }
}