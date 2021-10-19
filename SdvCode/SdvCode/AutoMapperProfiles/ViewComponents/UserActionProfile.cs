// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.ActivitiesComponent;

    public class UserActionProfile : Profile
    {
        public UserActionProfile()
        {
            this.CreateMap<UserAction, ActivitiesViewModel>();
        }
    }
}