using Microsoft.AspNetCore.Identity;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeService(ApplicationDbContext db,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(string role)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = role
            };

            IdentityResult result = await this.roleManager.CreateAsync(identityRole);
            return result;
        }

        public int GetRegisteredUsersCount()
        {
            return this.db.Users.Count();
        }
    }
}