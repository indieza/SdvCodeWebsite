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

    public class ProfileFollowersService : IProfileFollowersService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public ProfileFollowersService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public List<FollowersViewModel> ExtractFollowers(ApplicationUser user, string currentUserId)
        {
            var followers = this.db.FollowUnfollows
                .Where(x => x.ApplicationUserId == user.Id && x.IsFollowed == true)
                .Include(x => x.Follower)
                .Select(x => x.Follower)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<FollowersViewModel>>(followers);
            return model;
        }
    }
}