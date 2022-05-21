// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserActivitesDbUsage.FollowActivities
{
    using System.Linq;

    using SdvCode.Data;
    using SdvCode.Models.Enums;

    public class UserFollowActivitiesDbUsage : IUserFollowActivitiesDbUsage
    {
        private readonly ApplicationDbContext db;

        public UserFollowActivitiesDbUsage(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void DeleteFollowActivites()
        {
            var target = this.db.UserActions
                .Where(x => (x.BaseUserAction.ActionType == UserActionType.Follow ||
                x.BaseUserAction.ActionType == UserActionType.Followed ||
                x.BaseUserAction.ActionType == UserActionType.Unfollow ||
                x.BaseUserAction.ActionType == UserActionType.Unfollowed) &&
                x.BaseUserAction.ActionStatus == UserActionStatus.Read)
                .ToList();

            this.db.UserActions.RemoveRange(target);
            this.db.SaveChanges();
        }
    }
}