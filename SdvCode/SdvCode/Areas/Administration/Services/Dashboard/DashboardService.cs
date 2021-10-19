// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.Dashboard
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Administration.ViewModels.Dashboard;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class DashboardService : IDashboardService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public DashboardService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = db;
        }

        public DashboardViewModel GetDashboardInformation()
        {
            var usernames = this.db.Users.Select(x => x.UserName).ToList();
            var adminRol = this.db.Roles.FirstOrDefault(x => x.Name == Roles.Administrator.ToString());
            int adminsCount = 0;

            if (adminRol != null)
            {
                adminsCount = this.db.UserRoles.Count(x => x.RoleId == adminRol.Id);
            }

            int bannedPeople = this.db.Users.Count(x => x.IsBlocked == true);
            int postsCount = this.db.Posts.Count();
            int productsCount = this.db.Products.Count();
            int ordersCount = this.db.Orders.Count();

            return new DashboardViewModel
            {
                TotalUsersCount = usernames.Count(),
                TotalBannedUsers = bannedPeople,
                TotalBlogPosts = postsCount,
                TotalUsersInAdminRole = adminsCount,
                Usernames = usernames,
                TotalShopProducts = productsCount,
                TotalOrdersCount = ordersCount,
            };
        }

        public async Task<IdentityResult> CreateRole(string role)
        {
            Roles roleValue = (Roles)Enum.Parse(typeof(Roles), role);
            ApplicationRole identityRole = new ApplicationRole
            {
                Name = role,
                RoleLevel = (int)roleValue,
            };

            IdentityResult result = await this.roleManager.CreateAsync(identityRole);
            return result;
        }

        public async Task<bool> IsAddedUserInRole(string inputRole, string inputUsername)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(inputUsername);
            IdentityRole role = await this.roleManager.FindByNameAsync(inputRole);

            if (role == null)
            {
                IdentityResult result = await this.CreateRole(inputRole);

                if (result.Succeeded)
                {
                    role = await this.roleManager.FindByNameAsync(inputRole);
                }
                else
                {
                    return false;
                }
            }

            var isExist = this.db.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == role.Id);

            if (!isExist)
            {
                this.db.UserRoles.Add(new ApplicationUserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                });

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveUserFromRole(string username, string role)
        {
            var targetRole = await this.roleManager.FindByNameAsync(role);
            var targetUser = await this.userManager.FindByNameAsync(username);

            if (targetRole != null && targetUser != null)
            {
                var targetConnection = this.db.UserRoles.FirstOrDefault(x => x.RoleId == targetRole.Id && x.UserId == targetUser.Id);

                if (targetConnection != null)
                {
                    this.db.UserRoles.Remove(targetConnection);
                    await this.db.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> SyncFollowUnfollow()
        {
            var usersIds = this.db.Users.Select(x => x.Id).ToList();
            var targetNoneActiveRelations = this.db.FollowUnfollows
                .Where(x => !usersIds.Contains(x.FollowerId) || !usersIds.Contains(x.PersonId))
                .ToList();

            if (targetNoneActiveRelations.Count() > 0)
            {
                this.db.FollowUnfollows.RemoveRange(targetNoneActiveRelations);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}