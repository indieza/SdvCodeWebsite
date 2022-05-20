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

    public class AddNonCyclicActivity
    {
        private readonly ApplicationDbContext db;

        public AddNonCyclicActivity(ApplicationDbContext db)
        {
            this.db = db;
        }

        //public void AddUserAction(ApplicationUser user, Post post, BaseUserAction action, ApplicationUser postUser)
        //{
        //    this.db.UserActions.Add(new UserAction
        //    {
        //        BaseUserAction = action,
        //    });
        //}
    }
}