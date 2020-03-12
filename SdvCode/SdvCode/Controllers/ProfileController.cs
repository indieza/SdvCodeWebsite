// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Models;
    using SdvCode.Services;
    using SdvCode.ViewModels.Paging;
    using SdvCode.ViewModels.Profiles;
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

        [Route("/Profile/{username}")]
        public async Task<IActionResult> Index(string username)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            ApplicationUser user = this.profileService.ExtractUserInfo(username, currentUserId);
            bool hasAdmin = await this.profileService.HasAdmin();

            var model = new ProfileViewModel
            {
                ApplicationUser = user,
                HasAdmin = hasAdmin,
            };

            return this.View(model);
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

        [Route("/Profile/AllUsers")]
        public IActionResult AllUsers(int? page)
        {
            var currentUserId = this.userManager.GetUserId(this.HttpContext.User);
            var allUsers = this.profileService.GetAllUsers(currentUserId);

            var pageNumber = page ?? 1;
            this.ViewBag.UsersCards = allUsers.UsersCards.ToPagedList(pageNumber, GlobalConstants.UsersCountOnPage);
            return this.View();
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