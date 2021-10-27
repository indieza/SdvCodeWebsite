// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.DbUsage;
    using SdvCode.Areas.Administration.ViewModels.DbUsage.DeleteActivities;
    using SdvCode.Areas.Administration.ViewModels.DbUsage.DeleteUsersImages;
    using SdvCode.Constraints;
    using SdvCode.Models.Enums;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DbUsageController : Controller
    {
        private readonly IDbUsageService dbUsageService;

        public DbUsageController(IDbUsageService dbUsageService)
        {
            this.dbUsageService = dbUsageService;
        }

        public IActionResult DeleteUsersActivities()
        {
            return this.View();
        }

        public IActionResult DeleteUsersImages()
        {
            var model = new DeleteUsersImagesViewModel
            {
                Usernames = this.dbUsageService.GetAllUsernames(),
                DeleteUserImages = new DeleteImagesByUsernameInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActivityByName(DeleteActivitiesByNameInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                string activityText = model.ActivityName;
                string activityName = string.Join(string.Empty, activityText.Split(" "));

                UserActionType actionValue = (UserActionType)Enum.Parse(typeof(UserActionType), activityName);
                bool isRemoved = await this.dbUsageService.RemoveActivitiesByName(actionValue);

                if (isRemoved)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveActionByName, activityText);
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.NoActionsByGivenName, activityText);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("DeleteUsersActivities", "DbUsage", model);
            }

            return this.RedirectToAction("DeleteUsersActivities", "DbUsage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllActivities()
        {
            int count = await this.dbUsageService.RemoveAllActivities();

            if (count > 0)
            {
                this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveAllActions, count);
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.NoActionsForRemoving;
            }

            return this.RedirectToAction("DeleteUsersActivities", "DbUsage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserImages(DeleteUsersImagesViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string username = model.DeleteUserImages.Username;
                bool isDeleted = await this.dbUsageService.DeleteUserImagesByUsername(username);

                if (isDeleted)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveUserImages, username.ToUpper());
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.NoUserImagesByGivenUsername, username.ToUpper());
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("DeleteUsersImages", "DbUsage", model);
            }

            return this.RedirectToAction("DeleteUsersImages", "DbUsage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllusersImages()
        {
            int count = await this.dbUsageService.DeleteAllUsersImages();

            if (count > 0)
            {
                this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveAllUsersImages, count);
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.NoMoreUsersImagesForRemoving;
            }

            return this.RedirectToAction("DeleteUsersImages", "DbUsage");
        }
    }
}