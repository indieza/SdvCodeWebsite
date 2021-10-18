// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewModels.Profile.UserProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileZipCodeViewModel
    {
        public string Id { get; set; }

        public int Code { get; set; }

        public string CityId { get; set; }

        public ProfileCityViewModel City { get; set; }

        public ICollection<ProfileApplicationUserViewModel> ApplicationUsers { get; set; } = new HashSet<ProfileApplicationUserViewModel>();
    }
}