using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels.DashboardViewModels;
using SdvCode.Constraints;
using SdvCode.Data;
using SdvCode.Models;
using SdvCode.Services;

namespace SdvCode.Areas.Administration.Controllers
{
    [Area(GlobalConstants.AdministrationArea)]
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public IActionResult Index()
        {
            DashboardViewModel dashboard = this.dashboardService.GetDashboardInformation();
            DashboardIndexViewModel model = new DashboardIndexViewModel
            {
                DashboardViewModel = dashboard,
                CreateRole = new CreateRoleInputModel()
            };

            return View(model);
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> CreateRole(DashboardIndexViewModel model)
        {
            string role = model.CreateRole.Role;

            if (this.ModelState.IsValid)
            {
                IdentityResult result = await this.dashboardService.CreateRole(role);

                if (result.Succeeded)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedRole, role);
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.RoleExist, role);
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> AddUserInRole(DashboardIndexViewModel model)
        {
            string inputRole = model.AddUserInRole.Role;
            string inputUsername = model.AddUserInRole.Username;

            if (this.ModelState.IsValid)
            {
                var isAdded = await this.dashboardService.IsAddedUserInRole(inputRole, inputUsername);

                if (isAdded)
                {
                    TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedUserInRole, inputUsername.ToUpper(), inputRole);
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.UserAlreadyInRole, inputUsername.ToUpper(), inputRole);
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> RemoveUserFromRole(DashboardIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = model.RemoveUserFromRole.Username;
                var role = model.RemoveUserFromRole.Role;
                bool isRemoved = await this.dashboardService.RemoveUserFromRole(username, role);

                if (isRemoved)
                {
                    return this.Redirect($"/Profile/{username}");
                }
                else
                {
                    TempData["Error"] = string.Format(ErrorMessages.UserNotInRol, username.ToUpper(), role);
                }
            }
            else
            {
                TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> SyncFollowUnfollow()
        {
            bool isSync = await this.dashboardService.SyncFollowUnfollow();

            if (isSync)
            {
                TempData["Success"] = SuccessMessages.SuccessfullySyncFollowUnfollow;
            }
            else
            {
                TempData["Error"] = ErrorMessages.NoDataForSyncFollowUnfollow;
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}