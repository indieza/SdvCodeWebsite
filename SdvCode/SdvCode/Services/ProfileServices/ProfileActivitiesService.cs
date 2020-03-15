// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.ProfileServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;

    public class ProfileActivitiesService : IProfileActivitiesService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileActivitiesService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<ActivitiesViewModel>> ExtractActivities(string username)
        {
            List<ActivitiesViewModel> allActivities = new List<ActivitiesViewModel>();
            var user = await this.userManager.FindByNameAsync(username);
            var activities = this.db.UserActions.Where(x => x.ApplicationUserId == user.Id).ToList();

            foreach (var item in activities)
            {
                allActivities.Add(new ActivitiesViewModel
                {
                    Id = item.Id,
                    Action = item.Action,
                    ActionDate = item.ActionDate,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    CoverImageUrl = item.CoverImageUrl,
                    FollowerProfileImageUrl = item.FollowerProfileImageUrl,
                    FollowerUsername = item.FollowerUsername,
                    PersonProfileImageUrl = item.PersonProfileImageUrl,
                    PersonUsername = item.PersonUsername,
                    ProfileImageUrl = item.ProfileImageUrl,
                });
            }

            return allActivities.OrderByDescending(x => x.ActionDate).ToList();
        }
    }
}