// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Razor.Language.Intermediate;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users.ViewModels;

    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AddCyclicActivity cyclicActivity;

        public ProfileService(
            ApplicationDbContext db,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.cyclicActivity = new AddCyclicActivity(this.db);
        }

        public async Task DeleteActivity(ApplicationUser user)
        {
            var currentUserId = await this.userManager.GetUserIdAsync(user);
            var trash = this.db.UserActions.Where(x => x.ApplicationUserId == currentUserId).ToList();
            this.db.UserActions.RemoveRange(trash);
            await this.db.SaveChangesAsync();
        }

        public async Task<string> DeleteActivityById(ApplicationUser user, int activityId)
        {
            var currentUserId = await this.userManager.GetUserIdAsync(user);
            var trash = this.db.UserActions.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.Id == activityId);
            var activityName = trash.Action.ToString();
            this.db.UserActions.Remove(trash);
            await this.db.SaveChangesAsync();
            return activityName;
        }

        public async Task<ApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser currentUser)
        {
            var currentUserId = await this.userManager.GetUserIdAsync(currentUser);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            var group = new List<string>() { username, currentUser.UserName };

            var model = new ApplicationUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                RegisteredOn = user.RegisteredOn,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                AboutMe = user.AboutMe,
                ImageUrl = user.ImageUrl,
                CoverImageUrl = user.CoverImageUrl,
                IsBlocked = user.IsBlocked,
                State = this.db.States.FirstOrDefault(x => x.Id == user.StateId),
                Country = this.db.Countries.FirstOrDefault(x => x.Id == user.CountryId),
                City = this.db.Cities.FirstOrDefault(x => x.Id == user.CityId),
                ZipCode = this.db.ZipCodes.FirstOrDefault(x => x.Id == user.ZipCodeId),
                CountryCode = user.CountryCode,
                ActionsCount = this.db.UserActions.Count(x => x.ApplicationUserId == user.Id),
                IsFollowed = this.db.FollowUnfollows
                    .Any(x => x.FollowerId == currentUserId && x.PersonId == user.Id && x.IsFollowed == true),
                GitHubUrl = user.GitHubUrl,
                FacebookUrl = user.FacebookUrl,
                InstagramUrl = user.InstagramUrl,
                LinkedinUrl = user.LinkedinUrl,
                TwitterUrl = user.TwitterUrl,
                StackoverflowUrl = user.StackoverflowUrl,
                GroupName = string.Join(GlobalConstants.ChatGroupNameSeparator, group.OrderBy(x => x)),
            };

            var rolesIds = this.db.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
            var roles = this.db.Roles.Where(x => rolesIds.Contains(x.Id)).OrderBy(x => x.Name).ToList();
            model.Roles = roles.OrderBy(x => x.RoleLevel).ToList();

            return model;
        }

        public async Task<ApplicationUser> FollowUser(string username, ApplicationUser currentUser)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);

            if (!this.db.FollowUnfollows.Any(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id))
            {
                this.db.FollowUnfollows.Add(new FollowUnfollow
                {
                    PersonId = user.Id,
                    FollowerId = currentUser.Id,
                    IsFollowed = true,
                });
            }
            else
            {
                this.db.FollowUnfollows.FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id).IsFollowed = true;
            }

            this.cyclicActivity.AddUserAction(user, UserActionsType.Follow, currentUser);
            this.cyclicActivity.AddUserAction(currentUser, UserActionsType.Followed, user);
            await this.db.SaveChangesAsync();

            return currentUser;
        }

        public async Task<List<UserCardViewModel>> GetAllUsers(ApplicationUser user, string search)
        {
            List<UserCardViewModel> allUsers = new List<UserCardViewModel>();
            var currentUserId = await this.userManager.GetUserIdAsync(user);

            var targetUsers = new List<ApplicationUser>();

            if (search == null)
            {
                targetUsers = await this.db.Users.ToListAsync();
            }
            else
            {
                targetUsers = await this.db.Users
                     .Where(x => EF.Functions.Contains(x.UserName, search) ||
                     EF.Functions.Contains(x.FirstName, search) ||
                     EF.Functions.Contains(x.LastName, search))
                     .ToListAsync();
            }

            foreach (var targetUser in targetUsers)
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

            foreach (var targetUser in allUsers)
            {
                targetUser.FollowingsCount = await this.db.FollowUnfollows
                    .CountAsync(x => x.FollowerId == targetUser.UserId && x.IsFollowed == true);

                targetUser.FollowersCount = await this.db.FollowUnfollows
                    .CountAsync(x => x.PersonId == targetUser.UserId && x.IsFollowed == true);

                targetUser.HasFollowed = await this.db.FollowUnfollows
                    .AnyAsync(x => x.FollowerId == currentUserId && x.PersonId == targetUser.UserId && x.IsFollowed == true);

                targetUser.Activities = await this.db.UserActions
                    .CountAsync(x => x.ApplicationUserId == targetUser.UserId);
            }

            return allUsers;
        }

        public async Task<ApplicationUser> UnfollowUser(string username, ApplicationUser currentUser)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);

            if (this.db.FollowUnfollows.Any(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true))
            {
                this.db.FollowUnfollows
                    .FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true)
                    .IsFollowed = false;

                this.cyclicActivity.AddUserAction(user, UserActionsType.Unfollow, currentUser);
                this.cyclicActivity.AddUserAction(currentUser, UserActionsType.Unfollowed, user);
                await this.db.SaveChangesAsync();
            }

            return currentUser;
        }

        public async Task<bool> HasAdmin()
        {
            var role = await this.roleManager.FindByNameAsync(Roles.Administrator.ToString());

            if (role != null)
            {
                var roleId = role.Id;
                return this.db.UserRoles.Any(x => x.RoleId == roleId);
            }

            return false;
        }

        public void MakeYourselfAdmin(string username)
        {
            ApplicationUser user = this.db.Users.FirstOrDefault(x => x.UserName == username);
            ApplicationRole role = this.db.Roles.FirstOrDefault(x => x.Name == Roles.Administrator.ToString());

            if (user == null || role == null)
            {
                return;
            }

            this.db.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            this.db.SaveChanges();
        }

        public async Task<int> TakeCreatedPostsCountByUsername(string username)
        {
            return await this.db.Posts.CountAsync(x => x.ApplicationUser.UserName == username);
        }

        public async Task<int> TakeLikedPostsCountByUsername(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            return await this.db.PostsLikes.CountAsync(x => x.UserId == user.Id && x.IsLiked == true);
        }
    }
}