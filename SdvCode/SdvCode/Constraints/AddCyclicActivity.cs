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

    public class AddCyclicActivity
    {
        private readonly ApplicationDbContext db;

        public AddCyclicActivity(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddUserAction(ApplicationUser user, UserActionType action, ApplicationUser userPost)
        {
            if (this.db.UserActions
                    .Any(x => x.ActionType == action &&
                    x.ApplicationUserId == userPost.Id &&
                    x.PersonUsername == userPost.UserName &&
                    x.FollowerUsername == user.UserName))
            {
                var targetAction = this.db.UserActions
                    .FirstOrDefault(x => x.ActionType == action &&
                    x.ApplicationUserId == userPost.Id &&
                    x.PersonUsername == userPost.UserName &&
                    x.FollowerUsername == user.UserName);
                targetAction.ActionDate = DateTime.UtcNow;
                targetAction.ActionStatus = UserActionStatus.Unread;
            }
            else
            {
                this.db.UserActions.Add(new UserAction
                {
                    ActionType = action,
                    CreatedOn = DateTime.UtcNow,
                    ApplicationUserId = userPost.Id,
                    PersonUsername = userPost.UserName,
                    FollowerUsername = user.UserName,
                    ProfileImageUrl = user.ImageUrl,
                    ActionStatus = UserActionStatus.Unread,
                });
            }
        }

        public void AddLikeUnlikeActivity(ApplicationUser user, Post post, UserActionType action, ApplicationUser postUser)
        {
            if (this.db.UserActions
                .Any(x => x.PostId == post.Id &&
                x.ApplicationUserId == user.Id &&
                x.PersonUsername == user.UserName &&
                x.FollowerUsername == postUser.UserName &&
                x.ActionType == action))
            {
                var targetAction = this.db.UserActions
                    .FirstOrDefault(x => x.PostId == post.Id &&
                    x.ApplicationUserId == user.Id &&
                    x.PersonUsername == user.UserName &&
                    x.FollowerUsername == postUser.UserName &&
                    x.ActionType == action);
                targetAction.ActionDate = DateTime.UtcNow;
                targetAction.ActionStatus = UserActionStatus.Unread;
            }
            else
            {
                this.db.UserActions.Add(new UserAction
                {
                    ActionType = action,
                    CreatedOn = DateTime.UtcNow,
                    ApplicationUserId = user.Id,
                    PersonUsername = user.UserName,
                    FollowerUsername = postUser.UserName,
                    ProfileImageUrl = postUser.ImageUrl,
                    PostId = post.Id,
                    PostTitle = post.Title,
                    PostContent = post.ShortContent,
                    ActionStatus = UserActionStatus.Unread,
                });
            }
        }
    }
}