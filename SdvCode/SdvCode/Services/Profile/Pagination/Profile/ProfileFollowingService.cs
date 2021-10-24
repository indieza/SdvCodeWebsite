// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.Profile
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.ActivitiesComponent;

    public class ProfileFollowingService : IProfileFollowingService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public ProfileFollowingService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<FollowingViewModel>> ExtractFollowing(string username)
        {
            var followers = await this.db.FollowUnfollows
                .Where(x => x.Follower.UserName == username && x.IsFollowed == true)
                .Include(x => x.ApplicationUser)
                .Select(x => x.ApplicationUser)
                .AsSplitQuery()
                .ToListAsync();

            var model = this.mapper.Map<List<FollowingViewModel>>(followers);
            return model;
        }
    }
}