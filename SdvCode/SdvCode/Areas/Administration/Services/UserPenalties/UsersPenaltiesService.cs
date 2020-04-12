// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.UserPenalties
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class UsersPenaltiesService : IUsersPenaltiesService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersPenaltiesService(
            ApplicationDbContext db,
            RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
        }

        public async Task<bool> BlockUser(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);
            var adminRole = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole);

            if (this.db.UserRoles.Any(x => x.RoleId == adminRole.Id && x.UserId == user.Id))
            {
                return false;
            }

            if (user != null && user.IsBlocked == false)
            {
                user.IsBlocked = true;
                this.db.Users.Update(user);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UnblockUser(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);

            if (user != null && user.IsBlocked == true)
            {
                user.IsBlocked = false;
                this.db.Users.Update(user);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public ICollection<string> GetAllBlockedUsers()
        {
            return this.db.Users.Where(u => u.IsBlocked == true).Select(x => x.UserName).ToList();
        }

        public async Task<ICollection<string>> GetAllNotBlockedUsers()
        {
            var adminRole = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole);
            var usersAdminsIds = this.db.UserRoles
                .Where(x => x.RoleId == adminRole.Id)
                .Select(x => x.UserId)
                .ToList();
            return this.db.Users
                .Where(u => u.IsBlocked == false && !usersAdminsIds.Contains(u.Id))
                .Select(x => x.UserName)
                .ToList();
        }

        public async Task<int> BlockAllUsers()
        {
            var role = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole);
            int count = 0;

            if (role != null)
            {
                var noneAdminsIds = this.db.UserRoles.Where(x => x.RoleId != role.Id).Select(x => x.UserId).ToList();
                var users = this.db.Users.Where(x => noneAdminsIds.Contains(x.Id) && x.IsBlocked == false).ToList();
                count = users.Count();

                foreach (var user in users)
                {
                    user.IsBlocked = true;
                }

                this.db.Users.UpdateRange(users);
                await this.db.SaveChangesAsync();
                return count;
            }

            return count;
        }

        public async Task<int> UnblockAllUsers()
        {
            var users = this.db.Users.Where(x => x.IsBlocked == true).ToList();
            int count = users.Count();

            foreach (var user in users)
            {
                user.IsBlocked = false;
            }

            this.db.Users.UpdateRange(users);
            await this.db.SaveChangesAsync();
            return count;
        }
    }
}