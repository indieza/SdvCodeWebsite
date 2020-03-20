// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Constraints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class AddNonCyclicActivity
    {
        private readonly ApplicationDbContext db;

        public AddNonCyclicActivity(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddUserAction(ApplicationUser user, Post post, UserActionsType action, ApplicationUser postUser)
        {
            this.db.UserActions.Add(new UserAction
            {
                Action = action,
                ActionDate = DateTime.UtcNow,
                ApplicationUserId = user.Id,
                PersonUsername = user.UserName,
                FollowerUsername = postUser.UserName,
                ProfileImageUrl = postUser.ImageUrl,
                PostId = post.Id,
                PostTitle = post.Title,
                PostContent = post.ShortContent,
            });
        }
    }
}