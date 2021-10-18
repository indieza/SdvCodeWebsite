// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile.UserProfile;

    public class CityProfile : Profile
    {
        public CityProfile()
        {
            this.CreateMap<City, ProfileCityViewModel>();
        }
    }
}