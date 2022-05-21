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
    using SdvCode.Models.User.UserActions;

    public class AddCyclicActivity
    {
        private readonly ApplicationDbContext db;

        public AddCyclicActivity(ApplicationDbContext db)
        {
            this.db = db;
        }

        //public void AddUserAction(ApplicationUser user, BaseUserAction action, ApplicationUser userPost)
        //{
        //    if (this.db.UserActions
        //            .Any(x => x.BaseUserAction.ActionType == action.ActionType &&
        //            x.BaseUserAction.ApplicationUserId == userPost.Id &&
        //            x.BaseUserAction.ApplicationUser.UserName == userPost.UserName &&
        //            x.BaseUserAction.FollowerUsername == user.UserName))
        //    {
        //        var targetAction = this.db.UserActions
        //            .FirstOrDefault(x => x.BaseUserAction.ActionType == action.ActionType &&
        //            x.BaseUserAction.ApplicationUserId == userPost.Id &&
        //            x.BaseUserAction.ApplicationUser.UserName == userPost.UserName &&
        //            x.BaseUserAction.FollowerUsername == user.UserName);
        //        targetAction.ActionDate = DateTime.UtcNow;
        //        targetAction.ActionStatus = UserActionStatus.Unread;
        //    }
        //    else
        //    {
        //        this.db.UserActions.Add(new UserAction
        //        {
        //            BaseUserAction = action,
        //        });
        //    }
        //}

        //public void AddLikeUnlikeActivity(ApplicationUser user, Post post, BaseUserAction action, ApplicationUser postUser)
        //{
        //    if (this.db.UserActions
        //        .Any(x => x.PostId == post.Id &&
        //        x.BaseUserAction.ApplicationUserId == user.Id &&
        //        x.BaseUserAction.ApplicationUser.UserName == user.UserName &&
        //        x.BaseUserAction.FollowerUsername == postUser.UserName &&
        //        x.BaseUserAction.ActionType == action.ActionType))
        //    {
        //        var targetAction = this.db.UserActions
        //            .FirstOrDefault(x => x.PostId == post.Id &&
        //            x.BaseUserAction.ApplicationUserId == user.Id &&
        //            x.BaseUserAction.ApplicationUser.UserName == user.UserName &&
        //            x.BaseUserAction.FollowerUsername == postUser.UserName &&
        //            x.BaseUserAction.ActionType == action.ActionType);
        //        targetAction.ActionDate = DateTime.UtcNow;
        //        targetAction.ActionStatus = UserActionStatus.Unread;
        //    }
        //    else
        //    {
        //        this.db.UserActions.Add(new UserAction
        //        {
        //            BaseUserAction = action,
        //        });
        //    }
        //}
    }
}