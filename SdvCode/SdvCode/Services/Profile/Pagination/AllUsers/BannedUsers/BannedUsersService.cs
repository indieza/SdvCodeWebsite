// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.BannedUsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Users.ViewModels;

    public class BannedUsersService : IBannedUsersService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public BannedUsersService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<AllUsersUserCardViewModel>> ExtractAllUsers(string username, string search)
        {
            Expression<Func<ApplicationUser, bool>> usersFilter;
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

            var targetUsers = new List<ApplicationUser>();

            if (search == null)
            {
                usersFilter = x => x.IsBlocked;
            }
            else
            {
                usersFilter = x => (EF.Functions.FreeText(x.UserName, search) ||
                      EF.Functions.FreeText(x.FirstName, search) ||
                      EF.Functions.FreeText(x.LastName, search)) && x.IsBlocked;
            }

            var users = this.db.Users
                .Where(usersFilter)
                .Include(x => x.UserRoles)
                .Include(x => x.UserActions)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<AllUsersUserCardViewModel>>(users);
            return model;
        }
    }
}