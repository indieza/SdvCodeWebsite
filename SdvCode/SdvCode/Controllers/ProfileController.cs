// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.ProfileServices;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users;
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
            ApplicationUser user = this.profileService.ExtractUserInfo(username, this.HttpContext);
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
            ApplicationUser currentUser = await this.profileService.FollowUser(username, this.HttpContext);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyFollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Route("/Unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            ApplicationUser currentUser = await this.profileService.UnfollowUser(username, this.HttpContext);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnfollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Route("/Profile/AllUsers/{page?}")]
        public IActionResult AllUsers(int? page)
        {
            var allUsers = this.profileService.GetAllUsers(this.HttpContext);

            var pageNumber = page ?? 1;
            return this.View(allUsers.UsersCards.ToPagedList(pageNumber, GlobalConstants.UsersCountOnPage));
        }

        [Route("/DeleteActivityHistory/{username}")]
        public IActionResult DeleteActivityHistory(string username)
        {
            this.profileService.DeleteActivity(this.HttpContext);
            this.TempData["Success"] = SuccessMessages.SuccessfullyDeleteAllActivity;

            return this.Redirect($"/Profile/{username}");
        }

        [Route("/DeleteActivityById/{username}/{activityId}")]
        public async Task<IActionResult> DeleteActivityById(string username, int activityId)
        {
            string activityName = await this.profileService.DeleteActivityById(this.HttpContext, activityId);
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