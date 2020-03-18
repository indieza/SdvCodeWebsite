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
    using SdvCode.ViewModels.Paging;
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
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            ApplicationUser user = this.profileService.ExtractUserInfo(username, currentUserId);
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

        public IActionResult SwitchToTabs(string username, string tab)
        {
            var user = this.userManager.FindByNameAsync(username).Result;
            var vm = tab switch
            {
                "Activities" => ProfileTab.Activities,
                "Following" => ProfileTab.Following,
                "Followers" => ProfileTab.Followers,
                _ => ProfileTab.Activities,
            };

            return this.RedirectToAction("Index", new { username = user.UserName, tab = vm });
        }

        [Route("/Follow/{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            ApplicationUser currentUser = await this.profileService.FollowUser(username, currentUserId);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyFollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Route("/Unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            ApplicationUser currentUser = await this.profileService.UnfollowUser(username, currentUserId);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnfollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Route("/Profile/AllUsers/{page?}")]
        public IActionResult AllUsers(int? page)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            var allUsers = this.profileService.GetAllUsers(currentUserId);

            var pageNumber = page ?? 1;
            return this.View(allUsers.UsersCards.ToPagedList(pageNumber, GlobalConstants.UsersCountOnPage));
        }

        [Route("/DeleteActivityHistory/{username}")]
        public IActionResult DeleteActivityHistory(string username)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            this.profileService.DeleteActivity(currentUserId);
            this.TempData["Success"] = SuccessMessages.SuccessfullyDeleteAllActivity;

            return this.Redirect($"/Profile/{username}");
        }

        [Route("/DeleteActivityById/{username}/{activityId}")]
        public async Task<IActionResult> DeleteActivityById(string username, int activityId)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            string activityName = await this.profileService.DeleteActivityById(currentUserId, activityId);
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