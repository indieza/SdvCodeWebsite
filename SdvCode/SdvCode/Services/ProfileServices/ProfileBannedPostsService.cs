// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.ProfileServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;

    public class ProfileBannedPostsService : IProfileBannedPostsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileBannedPostsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<BannedPostsViewModel>> ExtractBannedPosts(ApplicationUser user, string currentUserId)
        {
            var currentUser = await this.userManager.FindByIdAsync(currentUserId);
            List<BannedPostsViewModel> bannedPostsModel = new List<BannedPostsViewModel>();
            List<BlockedPost> bannedPosts = new List<BlockedPost>();

            if (currentUser.UserName == user.UserName &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                 await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                bannedPosts = this.db.BlockedPosts.Where(x => x.IsBlocked == true).ToList();
            }
            else
            {
                bannedPosts = this.db.BlockedPosts.Where(x => x.IsBlocked == true && x.ApplicationUserId == user.Id).ToList();
            }

            foreach (var item in bannedPosts)
            {
                var currentPost = this.db.Posts.FirstOrDefault(x => x.Id == item.PostId);
                var currentCategoryName = this.db.Categories.FirstOrDefault(x => x.Id == currentPost.CategoryId);
                var author = await this.userManager.FindByIdAsync(currentPost.ApplicationUserId);

                bannedPostsModel.Add(new BannedPostsViewModel
                {
                    PostId = item.PostId,
                    CreatedOn = currentPost.CreatedOn,
                    PostContent = currentPost.ShortContent,
                    PostTitle = currentPost.Title,
                    Category = currentCategoryName,
                    AuthorUsername = author.UserName,
                    ImageUrl = author.ImageUrl,
                });
            }

            return bannedPostsModel.OrderByDescending(x => x.CreatedOn).ToList();
        }
    }
}