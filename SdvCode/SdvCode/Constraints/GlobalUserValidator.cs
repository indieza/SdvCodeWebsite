// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Constraints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class GlobalUserValidator
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public GlobalUserValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public GlobalUserValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public bool IsBlocked(ApplicationUser user)
        {
            return user.IsBlocked;
        }

        public bool IsInBlogRole(ApplicationUser user)
        {
            if (this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                this.userManager.IsInRoleAsync(user, Roles.Author.ToString()).Result ||
                this.userManager.IsInRoleAsync(user, Roles.Contributor.ToString()).Result ||
                this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result)
            {
                return true;
            }

            return false;
        }

        public bool IsInPostRole(ApplicationUser user, string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);

            if (post != null)
            {
                if (this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                    this.userManager.IsInRoleAsync(user, Roles.Author.ToString()).Result ||
                    post.ApplicationUserId == user.Id)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}