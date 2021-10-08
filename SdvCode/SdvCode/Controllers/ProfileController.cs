// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.Areas.Administration.Models.Enums;
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
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IProfileService profileService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IProfileService profileService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.profileService = profileService;
        }

        [HttpGet]
        [Route("Profile/{username}/{tab?}/{page?}")]
        public async Task<IActionResult> Index(string username, ProfileTab tab, int? page)
        {
            if (!await this.profileService.IsUserExist(username))
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUserViewModel user = await this.profileService.ExtractUserInfo(username, currentUser);

            var adminRole = await this.roleManager.FindByNameAsync(Roles.Administrator.ToString());
            bool hasAdmin = await this.profileService.HasAdmin(adminRole);

            var pageNumber = page ?? 1;

            var model = new ProfileViewModel
            {
                ApplicationUser = user,
                HasAdmin = hasAdmin,
                CreatedPosts = await this.profileService.TakeCreatedPostsCountByUsername(username),
                LikedPosts = await this.profileService.TakeLikedPostsCountByUsername(username),
                CommentsCount = await this.profileService.TakeCommentsCountByUsername(username),
                RatingScore = this.profileService.ExtractUserRatingScore(username),
                LatestScore = await this.profileService.GetLatestScore(currentUser, username),
            };

            // if (tab == 0)
            // {
            //    tab = ProfileTab.Activities;
            // }
            model.ActiveTab = tab;
            model.Page = pageNumber;

            return this.View(model);
        }

        public async Task<IActionResult> SwitchToAllActivitiesTabs(string username, string tab)
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

        public IActionResult SwitchToAllUsersTabs(string tab)
        {
            var vm = tab switch
            {
                "AllUsers" => AllUsersTab.AllUsers,
                "RecommendedUsers" => AllUsersTab.RecommendedUsers,
                "BannedUsers" => AllUsersTab.BannedUsers,
                "AllAdministrators" => AllUsersTab.AllAdministrators,
                _ => AllUsersTab.AllUsers,
            };

            return this.RedirectToAction("Users", new { tab = vm });
        }

        [HttpPost]
        [Route("/Follow/{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUser user = await this.profileService.FollowUser(username, currentUser);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyFollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{user.UserName}");
        }

        [HttpPost]
        [Route("/Unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ApplicationUser user = await this.profileService.UnfollowUser(username, currentUser);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnfollowedUser, username.ToUpper());

            return this.Redirect($"/Profile/{user.UserName}");
        }

        [HttpGet]
        [Route("/Profile/Users/{tab?}/{page?}/{search?}")]
        public IActionResult Users(AllUsersTab tab, int? page, string search)
        {
            var pageNumber = page ?? 1;

            if (search != null)
            {
                pageNumber = 1;
            }

            var model = new UsersViewModel
            {
                Search = search,
                ActiveTab = tab,
                Page = pageNumber,
            };

            return this.View(model);
        }

        [HttpPost]
        [Route("/RateUser")]
        public async Task<string> RateUser(string username, int rate)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            double rateUser = await this.profileService.RateUser(currentUser, username, rate);
            return $"{rateUser:F2}/5";
        }

        [HttpPost]
        [Route("/DeleteActivityHistory/{username}")]
        public async Task<IActionResult> DeleteActivityHistory(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            await this.profileService.DeleteActivity(currentUser);
            this.TempData["Success"] = SuccessMessages.SuccessfullyDeleteAllActivity;

            return this.Redirect($"/Profile/{username}");
        }

        [HttpPost]
        [Route("/DeleteActivityById/{username}/{activityId}")]
        public async Task<IActionResult> DeleteActivityById(string username, string activityId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            string activityName = await this.profileService.DeleteActivityById(currentUser, activityId);
            this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyDeletedActivityById, activityName);

            return this.Redirect($"/Profile/{username}");
        }

        [HttpPost]
        [Route("/Profile/{username}/changeActionStatus")]
        public async Task<string> ChangeActionStatus(string username, string id, string newStatus)
        {
            await this.profileService.ChangeActionStatus(username, id, newStatus);
            return newStatus;
        }

        [HttpPost]
        public IActionResult MakeYourselfAdmin(string username)
        {
            this.profileService.MakeYourselfAdmin(username);
            return this.Redirect($"/Profile/{username}");
        }
    }
}