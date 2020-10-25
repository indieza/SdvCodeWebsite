// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.UsersInformation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.UsersInformation;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class UsersInformationService : IUsersInformationService
    {
        private readonly ApplicationDbContext db;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersInformationService(
            ApplicationDbContext db,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<AllUsersViewModel> GetAllUsers()
        {
            var users = this.db.Users.ToList();
            var model = new AllUsersViewModel();

            foreach (var user in users)
            {
                var currentModel = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RegisteredOn = user.RegisteredOn,
                    EmailConfirmed = user.EmailConfirmed,
                    IsBlocked = user.IsBlocked,
                    PhoneNumber = user.PhoneNumber,
                    Country = await this.db.Countries.FirstOrDefaultAsync(x => x.Id == user.CountryId),
                    City = await this.db.Cities.FirstOrDefaultAsync(x => x.Id == user.CityId),
                    State = await this.db.States.FirstOrDefaultAsync(x => x.Id == user.StateId),
                    AboutMe = user.AboutMe,
                    CountryCode = user.CountryCode,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    ZipCode = await this.db.ZipCodes.FirstOrDefaultAsync(x => x.Id == user.ZipCodeId),
                };

                var userRoleNames = await this.userManager.GetRolesAsync(user);
                foreach (var roleName in userRoleNames)
                {
                    currentModel.Roles.Add(await this.db.Roles.FirstOrDefaultAsync(x => x.Name == roleName));
                }

                model.ApplicationUsers.Add(currentModel);
            }

            return model;
        }
    }
}