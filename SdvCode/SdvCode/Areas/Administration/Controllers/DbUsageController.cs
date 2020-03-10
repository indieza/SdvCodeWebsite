using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels.DbUsageViewModels;
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
        public IActionResult Index()
        {
            return View();
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

            return RedirectToAction("Index", "DbUsage");
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

            return RedirectToAction("Index", "DbUsage");
        }
    }
}