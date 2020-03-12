// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.ViewModels.UsersPenalties;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    public class UsersPenaltiesController : Controller
    {
        private readonly IUsersPenaltiesService usersPenaltiesService;

        public UsersPenaltiesController(IUsersPenaltiesService usersPenaltiesService)
        {
            this.usersPenaltiesService = usersPenaltiesService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult BlockUnblockUser()
        {
            var model = new UsersPenaltiesIndexModel
            {
                UsersPenaltiesViewModel = new UsersPenaltiesViewModel
                {
                    BlockedUsernames = this.usersPenaltiesService.GetAllBlockedUsers(),
                    NotBlockedUsernames = this.usersPenaltiesService.GetAllNotBlockedUsers(),
                },
                UsersPenaltiesInputModel = new UsersPenaltiesInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> BlockUser(UsersPenaltiesIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.BlockedUsername;
                bool isBlocked = await this.usersPenaltiesService.BlockUser(username);

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
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> UnblockUser(UsersPenaltiesIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.UnblockedUsername;
                bool isUnblocked = await this.usersPenaltiesService.UnblockUser(username);

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
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRole)]
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
    }
}