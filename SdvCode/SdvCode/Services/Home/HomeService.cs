// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper mapper;

        public HomeService(
            ApplicationDbContext db,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.mapper = mapper;
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

        public async Task<ICollection<HomeAdministratorUserViewModel>> GetAllAdministrators()
        {
            var role = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRole);
            var administratorsIds = this.db.UserRoles.Where(x => x.RoleId == role.Id).Select(x => x.UserId).ToList();
            var users = this.db.Users.Where(x => administratorsIds.Contains(x.Id)).ToList();
            var model = this.mapper.Map<List<HomeAdministratorUserViewModel>>(users);
            return model;
        }

        public async Task<ICollection<string>> GetHolidayThemeIcons()
        {
            var theme = await this.db.HolidayThemes
                .Include(x => x.HolidayIcons)
                .Where(x => x.IsActive)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            var result = new List<string>();

            if (theme != null)
            {
                result.AddRange(theme.HolidayIcons.Select(x => x.Url).ToList());
            }

            return result;
        }

        public ICollection<HomeLatestPostViewModel> GetLatestPosts()
        {
            var posts = this.db.Posts
                .Include(x => x.Category)
                .Include(x => x.ApplicationUser)
                .AsSplitQuery()
                .Where(x => x.PostStatus == PostStatus.Approved)
                .OrderByDescending(x => x.CreatedOn)
                .Take(GlobalConstants.LatestLayoutPostsCount)
                .ToList();

            var model = this.mapper.Map<List<HomeLatestPostViewModel>>(posts);
            return model;
        }

        public int GetPorductsCount()
        {
            return this.db.Products.Count(x => x.AvailableQuantity > 0);
        }

        public int GetPostsCount()
        {
            return this.db.Posts.Count(x => x.PostStatus == PostStatus.Approved);
        }

        public int GetRegisteredUsersCount()
        {
            return this.db.Users.Count();
        }
    }
}