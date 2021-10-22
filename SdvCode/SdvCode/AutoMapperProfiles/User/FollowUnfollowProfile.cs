// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.AutoMapperProfiles.User
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
    using SdvCode.ViewModels.Profile.UserViewComponents.ActivitiesComponent;

    public class FollowUnfollowProfile : Profile
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FollowUnfollowProfile(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.CreateMap<ApplicationUser, FollowingViewModel>()
                .ForMember(
                    dm => dm.HasFollow,
                    mo => mo.MapFrom(x => this.db.FollowUnfollows
                        .Any(y => y.FollowerId == userId &&
                            y.ApplicationUserId == x.Id &&
                            y.IsFollowed == true)));

            this.CreateMap<ApplicationUser, FollowersViewModel>()
                .ForMember(
                    dm => dm.HasFollow,
                    mo => mo.MapFrom(x => this.db.FollowUnfollows
                        .Any(y => y.FollowerId == userId &&
                            y.ApplicationUserId == x.Id &&
                            y.IsFollowed == true)));
        }
    }
}