// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.Profile
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Profile.UserViewComponents;

    public class ProfilePendingPostsService : IProfilePendingPostsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfilePendingPostsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<PendingPostsViewModel>> ExtractPendingPosts(ApplicationUser user, string currentUserId)
        {
            var currentUser = await this.userManager.FindByIdAsync(currentUserId);
            List<PendingPostsViewModel> pendingPostsModel = new List<PendingPostsViewModel>();
            List<PendingPost> pendingPosts = new List<PendingPost>();

            if (currentUser.UserName == user.UserName &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                 await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                pendingPosts = this.db.PendingPosts.Where(x => x.IsPending == true).ToList();
            }
            else
            {
                pendingPosts = this.db.PendingPosts.Where(x => x.IsPending == true && x.ApplicationUserId == user.Id).ToList();
            }

            foreach (var item in pendingPosts)
            {
                var currentPost = this.db.Posts.FirstOrDefault(x => x.Id == item.PostId);
                var currentCategoryName = this.db.Categories.FirstOrDefault(x => x.Id == currentPost.CategoryId);
                var author = await this.userManager.FindByIdAsync(currentPost.ApplicationUserId);

                pendingPostsModel.Add(new PendingPostsViewModel
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

            return pendingPostsModel.OrderByDescending(x => x.CreatedOn).ToList();
        }
    }
}