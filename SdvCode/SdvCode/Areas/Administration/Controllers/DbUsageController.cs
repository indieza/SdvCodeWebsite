using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteActivities;
using SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteUsersImages;
using SdvCode.Constraints;
using SdvCode.Models.Enums;

namespace SdvCode.Areas.Administration.Controllers
{
    [Area(GlobalConstants.AdministrationArea)]
    public class DbUsageController : Controller
    {
        private readonly IDbUsageService dbUsageService;

        public DbUsageController(IDbUsageService dbUsageService)
        {
            this.dbUsageService = dbUsageService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult DeleteUsersActivities()
        {
            return View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult DeleteUsersImages()
        {
            var model = new DeleteUsersImagesViewModel
            {
                Usernames = this.dbUsageService.GetAllUsernames(),
                DeleteUserImages = new DeleteImagesByUsernameInputModel()
            };

            return View(model);
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> DeleteActivityByName(DeleteActivitiesByNameInputModel model)
        {
            if (ModelState.IsValid)
            {
                string activityText = model.ActivityName;
                string activityName = string.Join("", activityText.Split(" "));

                UserActionsType actionValue = (UserActionsType)Enum.Parse(typeof(UserActionsType), activityName);
                bool isRemoved = await this.dbUsageService.RemoveActivitiesByName(actionValue);

                if (isRemoved)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveActionByName, activityText);
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.NoActionsByGivenName, activityText);
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("DeleteUsersActivities", "DbUsage");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> DeleteAllActivities()
        {
            int count = await this.dbUsageService.RemoveAllActivities();

            if (count > 0)
            {
                TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveAllActions, count);
            }
            else
            {
                TempData["Error"] = ErrorMessages.NoActionsForRemoving;
            }

            return RedirectToAction("DeleteUsersActivities", "DbUsage");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> DeleteUserImages(DeleteUsersImagesViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = model.DeleteUserImages.Username;
                bool isDeleted = await this.dbUsageService.DeleteUserImagesByUsername(username);

                if (isDeleted)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveUserImages, username.ToUpper());
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.NoUserImagesByGivenUsername, username.ToUpper());
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("DeleteUsersImages", "DbUsage");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> DeleteAllusersImages()
        {
            int count = await this.dbUsageService.DeleteAllUsersImages();

            if (count > 0)
            {
                TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveAllUsersImages, count);
            }
            else
            {
                TempData["Error"] = ErrorMessages.NoMoreUsersImagesForRemoving;
            }

            return RedirectToAction("DeleteUsersImages", "DbUsage");
        }
    }
}