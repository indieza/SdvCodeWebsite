// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models;

    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeService(
            ApplicationDbContext db,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(string role)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = role,
            };

            IdentityResult result = await this.roleManager.CreateAsync(identityRole);
            return result;
        }

        public ICollection<ApplicationUser> GetAllAdministrators()
        {
            var roleId = this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole).Result.Id;
            var administratorsIds = this.db.UserRoles.Where(x => x.RoleId == roleId).Select(x => x.UserId).ToList();
            var users = this.db.Users.Where(x => administratorsIds.Contains(x.Id)).ToList();
            return users;
        }

        public int GetRegisteredUsersCount()
        {
            return this.db.Users.Count();
        }
    }
}