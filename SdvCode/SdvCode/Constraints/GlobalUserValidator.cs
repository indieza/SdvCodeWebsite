// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Constraints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
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

        public async Task<bool> IsBlocked(HttpContext httpContext)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
            return user.IsBlocked;
        }

        public async Task<bool> IsInBlogRole(HttpContext httpContext)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Contributor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsInPostRole(HttpContext httpContext, string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var user = await this.userManager.GetUserAsync(httpContext.User);

            if (post != null)
            {
                if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                    await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()) ||
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