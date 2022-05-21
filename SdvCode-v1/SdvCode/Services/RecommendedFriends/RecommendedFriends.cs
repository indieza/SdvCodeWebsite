﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.RecommendedFriends
{
    using System.Linq;

    using SdvCode.Data;
    using SdvCode.Models.User;

    public class RecommendedFriends : IRecommendedFriends
    {
        private readonly ApplicationDbContext db;

        public RecommendedFriends(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddRecomendedFriends()
        {
            var trash = this.db.RecommendedFriends.ToList();
            this.db.RemoveRange(trash);
            this.db.SaveChanges();

            var users = this.db.Users.Where(x => !x.IsBlocked).ToList();

            foreach (var user in users)
            {
                var recommendedUsers = this.db.Users
                    .Where(x => x.StateId == user.StateId && x.Id != user.Id && x.IsBlocked == false)
                    .ToList();

                foreach (var recommendedUser in recommendedUsers)
                {
                    var followInfollow = this.db.FollowUnfollows
                        .FirstOrDefault(x => x.FollowerId == user.Id && x.ApplicationUserId == recommendedUser.Id && x.IsFollowed == true);

                    if (followInfollow == null)
                    {
                        user.RecommendedFriends.Add(new RecommendedFriend
                        {
                            ApplicationUserId = user.Id,
                            RecommendedApplicationUserId = recommendedUser.Id,
                        });
                    }
                }
            }

            this.db.SaveChanges();
        }
    }
}