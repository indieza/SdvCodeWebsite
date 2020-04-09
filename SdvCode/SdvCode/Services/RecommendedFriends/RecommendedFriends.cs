// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.RecommendedFriends
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
                foreach (var recommendedUser in this.db.Users.Where(x => x.StateId == user.StateId).ToList())
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

            this.db.SaveChanges();
        }
    }
}