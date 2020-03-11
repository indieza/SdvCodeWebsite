using Microsoft.AspNetCore.Identity;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Data;
using SdvCode.Models;
using SdvCode.Models.Enums;
using SdvCode.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;

        public ProfileService(ApplicationDbContext db,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
        }

        public void DeleteActivity(string currentUserId)
        {
            var trash = this.db.UserActions.Where(x => x.ApplicationUserId == currentUserId).ToList();
            this.db.UserActions.RemoveRange(trash);
            this.db.SaveChanges();
        }

        public string DeleteActivityById(string currentUserId, int activityId)
        {
            var trash = this.db.UserActions.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.Id == activityId);
            var activityName = trash.Action.ToString();
            this.db.UserActions.Remove(trash);
            this.db.SaveChanges();
            return activityName;
        }

        public ApplicationUser ExtractUserInfo(string username, string currentUserId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            user.IsFollowed = this.db.FollowUnfollows.Any(x => x.FollowerId == currentUserId && x.PersonId == user.Id && x.IsFollowed == true);

            var followersIds = this.db.FollowUnfollows
                .Where(x => x.PersonId == user.Id && x.IsFollowed == true)
                .Select(x => x.FollowerId)
                .ToList();
            var followingsIds = this.db.FollowUnfollows
                .Where(x => x.FollowerId == user.Id && x.IsFollowed == true)
                .Select(x => x.PersonId)
                .ToList();

            user.Followers = this.db.Users.Where(x => followersIds.Contains(x.Id)).ToList();
            foreach (var follower in user.Followers)
            {
                if (this.db.FollowUnfollows.Any(x => x.FollowerId == currentUserId && x.PersonId == follower.Id && x.IsFollowed == true))
                {
                    follower.HasFollow = true;
                }
            }

            user.Followings = this.db.Users.Where(x => followingsIds.Contains(x.Id)).ToList();
            foreach (var following in user.Followings)
            {
                if (this.db.FollowUnfollows.Any(x => x.FollowerId == currentUserId && x.PersonId == following.Id && x.IsFollowed == true))
                {
                    following.HasFollow = true;
                }
            }

            user.UserActions = this.db.UserActions
                .Where(x => x.ApplicationUserId == user.Id)
                .Select(x => new UserAction
                {
                    Id = x.Id,
                    Action = x.Action,
                    ActionDate = x.ActionDate,
                    PersonUsername = x.PersonUsername,
                    PersonProfileImageUrl = x.PersonProfileImageUrl ?? "/images/NoAvatarProfileImage.png",
                    FollowerUsername = x.FollowerUsername,
                    FollowerProfileImageUrl = x.FollowerProfileImageUrl ?? "/images/NoAvatarProfileImage.png",
                    ProfileImageUrl = x.ProfileImageUrl,
                    CoverImageUrl = x.CoverImageUrl
                })
                .OrderByDescending(x => x.ActionDate)
                .ToList();

            return user;
        }

        public ApplicationUser FollowUser(string username, string currentUserId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            var currentUser = this.db.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (!db.FollowUnfollows.Any(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id))
            {
                db.FollowUnfollows.Add(new FollowUnfollow
                {
                    PersonId = user.Id,
                    FollowerId = currentUser.Id,
                    IsFollowed = true
                });
            }
            else
            {
                db.FollowUnfollows.FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id).IsFollowed = true;
            }

            this.db.SaveChanges();

            var targetFollowerEntity = this.db.UserActions.FirstOrDefault(x =>
            x.Action == UserActionsType.Follow &&
            x.FollowerUsername == currentUser.UserName &&
            x.PersonUsername == username &&
            x.ApplicationUserId == currentUser.Id);

            if (targetFollowerEntity == null)
            {
                currentUser.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.Follow,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = username,
                    PersonProfileImageUrl = user.ImageUrl,
                    FollowerUsername = currentUser.UserName,
                    FollowerProfileImageUrl = currentUser.ImageUrl,
                    ApplicationUserId = currentUser.Id
                });

                this.db.SaveChanges();
            }
            else
            {
                targetFollowerEntity.ActionDate = DateTime.UtcNow;
                this.db.UserActions.Update(targetFollowerEntity);
                this.db.SaveChanges();
            }

            var targetPersonEntity = this.db.UserActions.FirstOrDefault(x => x.Action == UserActionsType.Followed &&
            x.FollowerUsername == currentUser.UserName &&
            x.PersonUsername == user.UserName &&
            x.ApplicationUserId == user.Id);

            if (targetPersonEntity == null)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.Followed,
                    ActionDate = DateTime.UtcNow,
                    FollowerUsername = currentUser.UserName,
                    FollowerProfileImageUrl = currentUser.ImageUrl,
                    PersonUsername = user.UserName,
                    PersonProfileImageUrl = user.ImageUrl,
                    ApplicationUserId = user.Id
                });

                this.db.SaveChanges();
            }
            else
            {
                targetPersonEntity.ActionDate = DateTime.UtcNow;
                this.db.UserActions.Update(targetPersonEntity);
                this.db.SaveChanges();
            }

            return currentUser;
        }

        public AllUsersViewModel GetAllUsers(string currentUserId)
        {
            var users = new AllUsersViewModel();

            foreach (var user in this.db.Users)
            {
                users.UsersCards.Add(new UserCardViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = user.ImageUrl,
                    CoverImageUrl = user.CoverImageUrl
                });
            }

            foreach (var user in users.UsersCards)
            {
                user.FollowingsCount = this.db.FollowUnfollows
                    .Count(x => x.FollowerId == user.UserId && x.IsFollowed == true);

                user.FollowersCount = this.db.FollowUnfollows
                    .Count(x => x.PersonId == user.UserId && x.IsFollowed == true);

                user.HasFollowed = this.db.FollowUnfollows
                    .Any(x => x.FollowerId == currentUserId && x.PersonId == user.UserId && x.IsFollowed == true);

                user.Activities = this.db.UserActions
                    .Count(x => x.ApplicationUserId == user.UserId);
            }

            return users;
        }

        public ApplicationUser UnfollowUser(string username, string currentUserId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            var currentUser = this.db.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (this.db.FollowUnfollows.Any(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true))
            {
                this.db.FollowUnfollows
                    .FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == currentUser.Id && x.IsFollowed == true)
                    .IsFollowed = false;

                db.SaveChanges();

                var targetUnfollowerEntity = this.db.UserActions.FirstOrDefault(x =>
                x.Action == UserActionsType.Unfollow &&
                x.FollowerUsername == currentUser.UserName &&
                x.PersonUsername == username &&
                x.ApplicationUserId == currentUser.Id);

                if (targetUnfollowerEntity == null)
                {
                    currentUser.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.Unfollow,
                        ActionDate = DateTime.UtcNow,
                        PersonUsername = username,
                        PersonProfileImageUrl = user.ImageUrl,
                        FollowerUsername = currentUser.UserName,
                        FollowerProfileImageUrl = currentUser.ImageUrl,
                        ApplicationUserId = currentUser.Id
                    });

                    this.db.SaveChanges();
                }
                else
                {
                    targetUnfollowerEntity.ActionDate = DateTime.UtcNow;
                    this.db.UserActions.Update(targetUnfollowerEntity);
                    this.db.SaveChanges();
                }

                var targetPersonEntity = this.db.UserActions.FirstOrDefault(x =>
                x.Action == UserActionsType.Unfollowed &&
                x.FollowerUsername == currentUser.UserName &&
                x.PersonUsername == user.UserName &&
                x.ApplicationUserId == user.Id);

                if (targetPersonEntity == null)
                {
                    user.UserActions.Add(new UserAction
                    {
                        Action = UserActionsType.Unfollowed,
                        ActionDate = DateTime.UtcNow,
                        FollowerUsername = currentUser.UserName,
                        FollowerProfileImageUrl = currentUser.ImageUrl,
                        PersonUsername = user.UserName,
                        PersonProfileImageUrl = user.ImageUrl,
                        ApplicationUserId = user.Id
                    });

                    this.db.SaveChanges();
                }
                else
                {
                    targetPersonEntity.ActionDate = DateTime.UtcNow;
                    this.db.UserActions.Update(targetPersonEntity);
                    this.db.SaveChanges();
                }
            }

            return currentUser;
        }

        public bool HasAdmin()
        {
            var role = this.roleManager.FindByNameAsync(Roles.Administrator.ToString()).Result;

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
            IdentityRole role = this.db.Roles.FirstOrDefault(x => x.Name == Roles.Administrator.ToString());

            if (user == null || role == null)
            {
                return;
            }

            this.db.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id
            });

            this.db.SaveChanges();
        }
    }
}