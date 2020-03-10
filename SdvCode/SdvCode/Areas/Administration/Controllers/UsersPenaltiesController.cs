using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels.UsersPenalties;
using SdvCode.Constraints;
using SdvCode.ViewModels.Users;

namespace SdvCode.Areas.Administration.Controllers
{
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
                    NotBlockedUsernames = this.usersPenaltiesService.GetAllNotBlockedUsers()
                },
                UsersPenaltiesInputModel = new UsersPenaltiesInputModel()
            };

            return View(model);
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> BlockUser(UsersPenaltiesIndexModel model)
        {
            if (ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.BlockedUsername;
                bool isBlocked = await this.usersPenaltiesService.BlockUser(username);

                if (isBlocked)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyBlockedUser, username.ToUpper());
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.UserAlreadyBlocked, username.ToUpper());
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> UnblockUser(UsersPenaltiesIndexModel model)
        {
            if (ModelState.IsValid)
            {
                string username = model.UsersPenaltiesInputModel.UnblockedUsername;
                bool isUnblocked = await this.usersPenaltiesService.UnblockUser(username);

                if (isUnblocked)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyUnblockedUser, username.ToUpper());
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.UserAlreadyUnblocked, username.ToUpper());
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("BlockUnblockUser", "UsersPenalties");
        }
    }
}