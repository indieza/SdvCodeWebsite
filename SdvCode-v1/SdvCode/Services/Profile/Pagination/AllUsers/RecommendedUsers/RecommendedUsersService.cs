// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.RecommendedUsers
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
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Users.ViewModels;

    public class RecommendedUsersService : IRecommendedUsersService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public RecommendedUsersService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<AllUsersUserCardViewModel>> ExtractAllUsers(string username, string search)
        {
            Expression<Func<RecommendedFriend, bool>> usersFilter;

            if (search == null)
            {
                usersFilter = x => x.ApplicationUser.UserName == username;
            }
            else
            {
                usersFilter = x => (EF.Functions.FreeText(x.RecommendedApplicationUser.UserName, search) ||
                    EF.Functions.FreeText(x.RecommendedApplicationUser.FirstName, search) ||
                     EF.Functions.FreeText(x.RecommendedApplicationUser.LastName, search)) &&
                     x.ApplicationUser.UserName == username;
            }

            var users = await this.db.RecommendedFriends
                .Where(usersFilter)
                .Include(x => x.RecommendedApplicationUser)
                .Select(x => x.RecommendedApplicationUser)
                .AsSplitQuery()
                .ToListAsync();

            var model = this.mapper.Map<List<AllUsersUserCardViewModel>>(users);
            return model;
        }
    }
}