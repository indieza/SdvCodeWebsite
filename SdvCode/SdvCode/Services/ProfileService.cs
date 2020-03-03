using SdvCode.Data;
using SdvCode.Models;
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
            user.Followings = this.db.Users.Where(x => followingsIds.Contains(x.Id)).ToList();
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