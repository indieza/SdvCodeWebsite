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
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
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

        public async Task<bool> IsInBlogRole(ApplicationUser user)
        {
            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Contributor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsInPostRole(ApplicationUser user, string id)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);

            if (post != null)
            {
                if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                    await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                    post.ApplicationUserId == user.Id)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> IsInCommentRole(ApplicationUser user, string id)
        {
            var comment = this.db.Comments.FirstOrDefault(x => x.Id == id);

            if (comment != null)
            {
                if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                    await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                    comment.ApplicationUserId == user.Id)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> IsPostApproved(string id, ApplicationUser user)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id);
            var userPostsIds = this.db.Posts.Where(x => x.ApplicationUserId == user.Id).Select(x => x.Id).ToList();

            if (post.PostStatus == PostStatus.Approved)
            {
                return true;
            }

            if (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()) ||
                userPostsIds.Contains(id))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsPostBlockedOrPending(string id)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post.PostStatus == PostStatus.Banned || post.PostStatus == PostStatus.Pending)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsPostBlocked(string id, ApplicationUser user)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post.PostStatus == PostStatus.Banned &&
                !await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) &&
                !await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString()))
            {
                return true;
            }

            return false;
        }
    }
}