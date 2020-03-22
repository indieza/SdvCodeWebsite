// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;

    public class ProfileFollowingService : IProfileFollowingService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileFollowingService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<FollowingViewModel>> ExtractFollowing(ApplicationUser user, string currentUserId)
        {
            List<FollowingViewModel> allFollowing = new List<FollowingViewModel>();
            var followers = this.db.FollowUnfollows.Where(x => x.FollowerId == user.Id && x.IsFollowed == true).ToList();

            foreach (var item in followers)
            {
                var follower = await this.userManager.FindByIdAsync(item.PersonId);
                var hasFollow = this.db.FollowUnfollows
                    .Any(x => x.FollowerId == currentUserId &&
                    x.PersonId == follower.Id && x.IsFollowed == true);

                allFollowing.Add(new FollowingViewModel
                {
                    Username = follower.UserName,
                    FirstName = follower.FirstName,
                    LastName = follower.LastName,
                    ImageUrl = follower.ImageUrl,
                    IsBlocked = follower.IsBlocked,
                    HasFollow = hasFollow,
                });
            }

            return allFollowing;
        }
    }
}