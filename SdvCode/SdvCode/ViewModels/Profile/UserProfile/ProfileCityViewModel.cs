// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileCityViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public ProfileStateViewModel State { get; set; }

        public string CountryId { get; set; }

        public ProfileCountryViewModel Country { get; set; }

        public ICollection<ProfileZipCodeViewModel> ZipCodes { get; set; } = new HashSet<ProfileZipCodeViewModel>();

        public ICollection<ProfileApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ProfileApplicationUserViewModel>();
    }
}