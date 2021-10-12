// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class UserProfile : Profile
    {
        private readonly ApplicationDbContext db;

        public UserProfile(ApplicationDbContext db)
        {
            this.db = db;

            this.CreateMap<ZipCode, ZipCodeViewModel>();
            this.CreateMap<CountryCode, CountryCodeViewModel>();
            this.CreateMap<State, StateViewModel>();
            this.CreateMap<City, CityViewModel>();
            this.CreateMap<Country, CountryViewModel>();
            this.CreateMap<ApplicationUser, ApplicationUserViewModel>();
        }
    }
}