using Microsoft.AspNetCore.Identity;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Areas.Administration.ViewModels;
using SdvCode.Data;
using SdvCode.Models;
using SdvCode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public DashboardService(RoleManager<IdentityRole> roleManager,
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

            return new DashboardViewModel
            {
                TotalUsersCount = usernames.Count(),
                TotalBannedUsers = 10,
                TotalBlogPosts = 12,
                TotalUsersInAdminRole = adminsCount,
                Usernames = usernames
            };
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
                this.db.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });

                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}