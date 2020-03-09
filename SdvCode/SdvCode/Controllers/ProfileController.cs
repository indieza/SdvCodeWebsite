using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Data;
using SdvCode.Models;
using SdvCode.Services;
using SdvCode.ViewModels.Profiles;
using SdvCode.ViewModels.Users;

namespace SdvCode.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileService profileService;

        public ProfileController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }

        [Authorize]
        [Route("/Profile/{username}")]
        public IActionResult Index(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            ApplicationUser user = this.profileService.ExtractUserInfo(username, currentUserId);
            bool hasAdmin = this.profileService.HasAdmin();

            var model = new ProfileViewModel
            {
                ApplicationUser = user,
                HasAdmin = hasAdmin
            };

            return View(model);
        }

        [Authorize]
        [Route("/Follow/{username}")]
        public IActionResult Follow(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            ApplicationUser currentUser = this.profileService.FollowUser(username, currentUserId);

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Authorize]
        [Route("/Unfollow/{username}")]
        public IActionResult Unfollow(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            ApplicationUser currentUser = this.profileService.UnfollowUser(username, currentUserId);

            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Authorize]
        [Route("/Profile/AllUsers")]
        public IActionResult AllUsers()
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            var allUsers = this.profileService.GetAllUsers(currentUserId);
            return this.View(allUsers);
        }

        [Authorize]
        [Route("/DeleteActivityHistory/{username}")]
        public IActionResult DeleteActivityHistory(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            this.profileService.DeleteActivity(currentUserId);

            return this.Redirect($"/Profile/{username}");
        }

        [Authorize]
        [Route("/DeleteActivityById/{username}/{activityId}")]
        public IActionResult DeleteActivityById(string username, int activityId)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            this.profileService.DeleteActivityById(currentUserId, activityId);

            return this.Redirect($"/Profile/{username}");
        }

        [Authorize]
        public IActionResult MakeYourselfAdmin(string username)
        {
            this.profileService.MakeYourselfAdmin(username);
            return this.Redirect($"/Profile/{username}");
        }
    }
}