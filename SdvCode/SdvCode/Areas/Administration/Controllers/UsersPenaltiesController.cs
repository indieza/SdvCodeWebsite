// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.UserPenalties;
    using SdvCode.Areas.Administration.ViewModels.UsersPenalties;
    using SdvCode.Constraints;
    using SdvCode.Models.User;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class UsersPenaltiesController : Controller
    {
        private readonly IUsersPenaltiesService usersPenaltiesService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersPenaltiesController(
            IUsersPenaltiesService usersPenaltiesService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersPenaltiesService = usersPenaltiesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> BlockUnblockUser()
        {
            var model = new UsersPenaltiesIndexModel
            {
                UsersPenaltiesViewModel = new UsersPenaltiesViewModel
                {
                    BlockedUsernames = this.usersPenaltiesService.GetAllBlockedUsers(),
                    NotBlockedUsernames = await this.usersPenaltiesService.GetAllNotBlockedUsers(),
                },
                UsersPenaltiesInputModel = new UsersPenaltiesInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(UsersPenaltiesIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.BlockedUsername;
                var currentUser = await this.userManager.GetUserAsync(this.User);
                bool isBlocked = await this.usersPenaltiesService.BlockUser(username, currentUser);

                if (isBlocked)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyBlockedUser, username.ToUpper());
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.UserAlreadyBlocked, username.ToUpper());
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(UsersPenaltiesIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.UnblockedUsername;
                var currentUser = await this.userManager.GetUserAsync(this.User);
                bool isUnblocked = await this.usersPenaltiesService.UnblockUser(username, currentUser);

                if (isUnblocked)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnblockedUser, username.ToUpper());
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.UserAlreadyUnblocked, username.ToUpper());
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }

        [HttpPost]
        public async Task<IActionResult> BlockAllUsers()
        {
            int count = await this.usersPenaltiesService.BlockAllUsers();

            if (count > 0)
            {
                this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyBlockedAllUsers, count);
            }
            else
            {
                this.TempData["Error"] = string.Format(ErrorMessages.AllUsersAlreadyBlocked);
            }

            return this.RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockAllUsers()
        {
            int count = await this.usersPenaltiesService.UnblockAllUsers();

            if (count > 0)
            {
                this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnblockedAllUsers, count);
            }
            else
            {
                this.TempData["Error"] = string.Format(ErrorMessages.AllUsersAlreadyUnblocked);
            }

            return this.RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }

        public IActionResult HangFire()
        {
            return this.RedirectToPage("/Administration/UsersPenalties/HangFire");
        }
    }
}