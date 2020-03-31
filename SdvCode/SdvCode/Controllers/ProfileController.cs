// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users;
    using SdvCode.ViewModels.Users.ViewModels;
    using X.PagedList;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileService profileService;

        public ProfileController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }

        [Route("Profile/{username}/{tab?}/{page?}")]
        public async Task<IActionResult> Index(string username, ProfileTab tab, int? page)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUserViewModel user = await this.profileService.ExtractUserInfo(username, currentUser);
            bool hasAdmin = await this.profileService.HasAdmin();

            var pageNumber = page ?? 1;

            var model = new ProfileViewModel
            {
                ApplicationUser = user,
                HasAdmin = hasAdmin,
                CreatedPosts = await this.profileService.TakeCreatedPostsCountByUsername(username),
                LikedPosts = await this.profileService.TakeLikedPostsCountByUsername(username),
            };

            // if (tab == 0)
            // {
            //    tab = ProfileTab.Activities;
            // }
            model.ActiveTab = tab;
            model.Page = pageNumber;

            return this.View(model);
        }

        public async Task<IActionResult> SwitchToTabs(string username, string tab)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var vm = tab switch
            {
                "Activities" => ProfileTab.Activities,
                "Following" => ProfileTab.Following,
                "Followers" => ProfileTab.Followers,
                "Favorites" => ProfileTab.Favorites,
                "PendingPosts" => ProfileTab.PendingPosts,
                "BannedPosts" => ProfileTab.BannedPosts,
                _ => ProfileTab.Activities,
            };

            return this.RedirectToAction("Index", new { username = user.UserName, tab = vm });
        }

        [Route("/Follow/{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUser user = await this.profileService.FollowUser(username, currentUser);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyFollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{user.UserName}");
        }

        [Route("/Unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUser user = await this.profileService.UnfollowUser(username, currentUser);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnfollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{user.UserName}");
        }

        [Route("/Profile/AllUsers/{page?}/{search?}")]
        public async Task<IActionResult> AllUsers(int? page, string search)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            List<UserCardViewModel> allUsers = await this.profileService.GetAllUsers(currentUser, search);

            var pageNumber = page ?? 1;

            if (search != null)
            {
                pageNumber = 1;
            }

            var model = new AllUsersViewModel
            {
                UsersCards = allUsers.ToPagedList(pageNumber, GlobalConstants.UsersCountOnPage),
                Search = search,
            };

            return this.View(model);
        }

        [Route("/DeleteActivityHistory/{username}")]
        public async Task<IActionResult> DeleteActivityHistory(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            this.profileService.DeleteActivity(currentUser);
            this.TempData["Success"] = SuccessMessages.SuccessfullyDeleteAllActivity;

            return this.Redirect($"/Profile/{username}");
        }

        [Route("/DeleteActivityById/{username}/{activityId}")]
        public async Task<IActionResult> DeleteActivityById(string username, int activityId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            string activityName = await this.profileService.DeleteActivityById(currentUser, activityId);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyDeletedActivityById, activityName);

            return this.Redirect($"/Profile/{username}");
        }

        public IActionResult MakeYourselfAdmin(string username)
        {
            this.profileService.MakeYourselfAdmin(username);
            return this.Redirect($"/Profile/{username}");
        }
    }
}