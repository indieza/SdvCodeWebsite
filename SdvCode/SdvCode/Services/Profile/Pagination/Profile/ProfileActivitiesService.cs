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

    public class ProfileActivitiesService : IProfileActivitiesService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ProfileActivitiesService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<List<ActivitiesViewModel>> ExtractActivities(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var activities = this.db.UserActions
                .Where(x => x.ApplicationUserId == user.Id)
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.CreatedOn)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<ActivitiesViewModel>>(activities);
            return model;
        }
    }
}