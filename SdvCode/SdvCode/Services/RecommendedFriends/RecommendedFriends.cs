// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.RecommendedFriends
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class RecommendedFriends : IRecommendedFriends
    {
        private readonly ApplicationDbContext db;

        public RecommendedFriends(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddRecomendedFrinds()
        {
            var trash = this.db.RecommendedFriends.ToList();
            this.db.RemoveRange(trash);
            this.db.SaveChanges();

            var users = this.db.Users.ToList();

            foreach (var user in users)
            {
                var recommendedUsers = this.db.Users
                    .Where(x => x.StateId == user.StateId && x.Id != user.Id && x.IsBlocked == false)
                    .ToList();

                foreach (var recommendedUser in recommendedUsers)
                {
                    var followUnfollow = this.db.FollowUnfollows
                        .FirstOrDefault(x => x.PersonId == user.Id && x.FollowerId == recommendedUser.Id && x.IsFollowed == false);

                    if (followUnfollow == null)
                    {
                        user.RecommendedFriends.Add(new RecommendedFriend
                        {
                            RecommendedUsername = recommendedUser.UserName,
                            RecommendedFirstName = recommendedUser.FirstName,
                            RecommendedLastName = recommendedUser.LastName,
                            RecommendedImageUrl = recommendedUser.ImageUrl,
                            RecommendedCoverImage = recommendedUser.CoverImageUrl,
                        });
                    }
                }
            }

            this.db.SaveChanges();
        }
    }
}