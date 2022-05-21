// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.AllAdministrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class AllAdministratorsService : IAllAdministratorsService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AllAdministratorsService(
            ApplicationDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<AllUsersUserCardViewModel>> ExtractAllUsers(string username, string search)
        {
            Expression<Func<ApplicationUser, bool>> usersFilter;

            if (search == null)
            {
                usersFilter = x => x.UserRoles.Any(x => x.Role.Name == GlobalConstants.AdministratorRole);
            }
            else
            {
                usersFilter = x => (EF.Functions.FreeText(x.UserName, search) ||
                     EF.Functions.FreeText(x.FirstName, search) ||
                     EF.Functions.FreeText(x.LastName, search)) &&
                     x.UserRoles.Any(y => y.Role.Name == GlobalConstants.AdministratorRole);
            }

            var users = await this.db.Users
                .Where(usersFilter)
                .Include(x => x.UserActions)
                .AsSplitQuery()
                .ToListAsync();

            var model = this.mapper.Map<List<AllUsersUserCardViewModel>>(users);
            return model;
        }
    }
}