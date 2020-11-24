// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Razor.Language.Intermediate;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users.ViewModels;

    public class ProfileService : AddCyclicActivity, IProfileService
    {
        private readonly ApplicationDbContext db;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly INotificationService notificationService;

        public ProfileService(
            ApplicationDbContext db,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService)
            : base(db)
        {
            this.db = db;
            this.notificationHubContext = notificationHubContext;
            this.notificationService = notificationService;
        }

        public async Task DeleteActivity(ApplicationUser user)
        {
            var trash = this.db.UserActions.Where(x => x.ApplicationUserId == user.Id).ToList();
            this.db.UserActions.RemoveRange(trash);
            await this.db.SaveChangesAsync();
        }

        public async Task<string> DeleteActivityById(ApplicationUser user, int activityId)
        {
            var trash = this.db.UserActions.FirstOrDefault(x => x.ApplicationUserId == user.Id && x.Id == activityId);
            var activityName = trash.Action.ToString();
            this.db.UserActions.Remove(trash);
            await this.db.SaveChangesAsync();
            return activityName;
        }

        public async Task<ApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser currentUser)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);
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
                CountryCode = this.db.CountryCodes.FirstOrDefault(x => x.Id == user.CountryCodeId),
                ActionsCount = this.db.UserActions.Count(x => x.ApplicationUserId == user.Id),
                IsFollowed = this.db.FollowUnfollows
                    .Any(x => x.FollowerId == currentUser.Id && x.PersonId == user.Id && x.IsFollowed == true),
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

            this.AddUserAction(user, UserActionsType.Follow, currentUser);
            this.AddUserAction(currentUser, UserActionsType.Followed, user);
            await this.db.SaveChangesAsync();

            return currentUser;
        }

        public async Task<ApplicationUser> UnfollowUser(string username, ApplicationUser currentUser)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);

            if (this.db.FollowUnfollows.Any(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true))
            {
                this.db.FollowUnfollows
                    .FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true)
                    .IsFollowed = false;

                this.AddUserAction(user, UserActionsType.Unfollow, currentUser);
                this.AddUserAction(currentUser, UserActionsType.Unfollowed, user);
                await this.db.SaveChangesAsync();
            }

            return currentUser;
        }

        public async Task<bool> HasAdmin(ApplicationRole role)
        {
            if (role != null)
            {
                var roleId = role.Id;
                return await this.db.UserRoles.AnyAsync(x => x.RoleId == roleId);
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

            if (this.db.UserRoles.Any(x => x.RoleId == role.Id))
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

        public async Task<int> TakeCommentsCountByUsername(string username)
        {
            return await this.db.Comments.CountAsync(x => x.ApplicationUser.UserName == username);
        }

        public async Task<double> RateUser(ApplicationUser currentUser, string username, int rate)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var targetRating = await this.db.UserRatings
                .FirstOrDefaultAsync(x => x.Username == username && x.RaterUsername == currentUser.UserName);

            if (targetRating != null)
            {
                targetRating.Stars = rate;
                this.db.Update(targetRating);
            }
            else
            {
                targetRating = new UserRating
                {
                    RaterUsername = currentUser.UserName,
                    Username = username,
                    Stars = rate,
                };
                this.db.UserRatings.Add(targetRating);
            }

            await this.db.SaveChangesAsync();

            if (currentUser.UserName != username)
            {
                string notificationId =
                       await this.notificationService
                       .AddProfileRatingNotification(user, currentUser, rate);

                var count = await this.notificationService.GetUserNotificationsCount(user.UserName);
                await this.notificationHubContext
                    .Clients
                    .User(user.Id)
                    .SendAsync("ReceiveNotification", count, true);

                var notificationForApproving = await this.notificationService.GetNotificationById(notificationId);
                await this.notificationHubContext.Clients.User(user.Id)
                    .SendAsync("VisualizeNotification", notificationForApproving);
            }

            return this.CalculateRatingScore(username);
        }

        public double ExtractUserRatingScore(string username)
        {
            return this.CalculateRatingScore(username);
        }

        public async Task<int> GetLatestScore(ApplicationUser currentUser, string username)
        {
            var target = await this.db.UserRatings
                .FirstOrDefaultAsync(x => x.Username == username && x.RaterUsername == currentUser.UserName);
            return target == null ? 0 : target.Stars;
        }

        public async Task<bool> IsUserExist(string username)
        {
            return await this.db.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task ChangeActionStatus(string username, int id, string newStatus)
        {
            var action = await this.db.UserActions
                .FirstOrDefaultAsync(x => x.Id == id && x.PersonUsername == username);

            if (action != null)
            {
                action.ActionStatus = (UserActionsStatus)Enum.Parse(typeof(UserActionsStatus), newStatus);
                this.db.UserActions.Update(action);
                await this.db.SaveChangesAsync();
            }
        }

        private double CalculateRatingScore(string username)
        {
            double score;
            var count = this.db.UserRatings.Count(x => x.Username == username);
            if (count != 0)
            {
                var totalScore = this.db.UserRatings.Where(x => x.Username == username).Sum(x => x.Stars);
                score = Math.Round((double)totalScore / count, 2);
                return score;
            }

            return 0;
        }
    }
}