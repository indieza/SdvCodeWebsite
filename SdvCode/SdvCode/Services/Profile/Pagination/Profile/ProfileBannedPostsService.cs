// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.Profile
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

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;
    using SdvCode.ViewModels.Profile.UserViewComponents.BlogComponent;

    public class ProfileBannedPostsService : IProfileBannedPostsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ProfileBannedPostsService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<List<BannedPostViewModel>> ExtractBannedPosts(ApplicationUser user, string currentUserId)
        {
            var currentUser = await this.userManager.FindByIdAsync(currentUserId);
            Expression<Func<BlockedPost, bool>> postsFilter;

            if (currentUser.UserName == user.UserName &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                 await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                postsFilter = x => x.IsBlocked == true;
            }
            else
            {
                postsFilter = x => x.IsBlocked == true && x.ApplicationUserId == user.Id;
            }

            var posts = this.db.BlockedPosts
                .Include(x => x.Post)
                .ThenInclude(x => x.Category)
                .Include(x => x.ApplicationUser)
                .Where(postsFilter)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<BannedPostViewModel>>(posts);
            return model;
        }
    }
}