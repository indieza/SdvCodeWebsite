using SdvCode.Data;
using SdvCode.Models;
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

        public ProfileService(ApplicationDbContext db)
        {
            this.db = db;
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
                    Country = user.Country,
                    City = user.City,
                    ImageUrl = user.ImageUrl,
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
            }

            return currentUser;
        }
    }
}