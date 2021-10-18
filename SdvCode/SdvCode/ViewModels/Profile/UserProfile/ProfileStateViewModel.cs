// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileStateViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryId { get; set; }

        public ProfileCountryViewModel Country { get; set; }

        public ICollection<ProfileCityViewModel> Cities { get; set; } = new HashSet<ProfileCityViewModel>();

        public ICollection<ProfileApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ProfileApplicationUserViewModel>();
    }
}