// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.AllAdministrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class AllAdministratorsService : IAllAdministratorsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public AllAdministratorsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<UserCardViewModel>> ExtractAllUsers(string username, string search)
        {
            List<UserCardViewModel> allUsers = new List<UserCardViewModel>();
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

            var targetUsers = new List<ApplicationUser>();

            if (search == null)
            {
                targetUsers = await this.db.Users.ToListAsync();
            }
            else
            {
                targetUsers = await this.db.Users
                     .Where(x => EF.Functions.FreeText(x.UserName, search) ||
                     EF.Functions.FreeText(x.FirstName, search) ||
                     EF.Functions.FreeText(x.LastName, search))
                     .ToListAsync();
            }

            foreach (var targetUser in targetUsers)
            {
                if (await this.userManager.IsInRoleAsync(targetUser, GlobalConstants.AdministratorRole))
                {
                    allUsers.Add(new UserCardViewModel
                    {
                        UserId = targetUser.Id,
                        Username = targetUser.UserName,
                        FirstName = targetUser.FirstName,
                        LastName = targetUser.LastName,
                        ImageUrl = targetUser.ImageUrl,
                        CoverImageUrl = targetUser.CoverImageUrl,
                    });
                }
            }

            foreach (var targetUser in allUsers)
            {
                targetUser.FollowingsCount = await this.db.FollowUnfollows
                    .CountAsync(x => x.FollowerId == targetUser.UserId && x.IsFollowed == true);

                targetUser.FollowersCount = await this.db.FollowUnfollows
                    .CountAsync(x => x.PersonId == targetUser.UserId && x.IsFollowed == true);

                targetUser.HasFollowed = await this.db.FollowUnfollows
                    .AnyAsync(x => x.FollowerId == user.Id && x.PersonId == targetUser.UserId && x.IsFollowed == true);

                targetUser.Activities = await this.db.UserActions
                    .CountAsync(x => x.ApplicationUserId == targetUser.UserId);
            }

            return allUsers;
        }
    }
}