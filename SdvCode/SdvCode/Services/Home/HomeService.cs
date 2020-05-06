// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml.ConditionalFormatting;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Home;

    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<ApplicationRole> roleManager;

        public HomeService(
            ApplicationDbContext db,
            RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
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

        public async Task<ICollection<ApplicationUser>> GetAllAdministrators()
        {
            var role = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole);
            var administratorsIds = this.db.UserRoles.Where(x => x.RoleId == role.Id).Select(x => x.UserId).ToList();
            var users = this.db.Users.Where(x => administratorsIds.Contains(x.Id)).ToList();
            return users;
        }

        public async Task<ICollection<LatestPostViewModel>> GetLatestPosts()
        {
            var result = new List<LatestPostViewModel>();
            var targetPosts = this.db.Posts
                .Where(x => x.PostStatus == PostStatus.Approved)
                .OrderByDescending(x => x.CreatedOn)
                .Take(GlobalConstants.LatestLayoutPostsCount)
                .ToList();

            foreach (var targetPost in targetPosts)
            {
                var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == targetPost.ApplicationUserId);
                var category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == targetPost.CategoryId);

                result.Add(new LatestPostViewModel
                {
                    Id = targetPost.Id,
                    CreatedOn = targetPost.CreatedOn,
                    Title = targetPost.Title,
                    ImageUrl = targetPost.ImageUrl,
                    CategoryId = targetPost.CategoryId,
                    CategoryName = category.Name,
                    CreatorUsername = user.UserName,
                });
            }

            return result;
        }

        public int GetRegisteredUsersCount()
        {
            return this.db.Users.Count();
        }
    }
}