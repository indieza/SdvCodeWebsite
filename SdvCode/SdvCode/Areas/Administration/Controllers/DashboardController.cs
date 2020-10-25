// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Areas.Administration.Services.Dashboard;
    using SdvCode.Areas.Administration.ViewModels.DashboardViewModels;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            DashboardViewModel dashboard = this.dashboardService.GetDashboardInformation();
            DashboardIndexViewModel model = new DashboardIndexViewModel
            {
                DashboardViewModel = dashboard,
                CreateRole = new CreateRoleInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(DashboardIndexViewModel model)
        {
            string role = model.CreateRole.Role;

            if (this.ModelState.IsValid)
            {
                IdentityResult result = await this.dashboardService.CreateRole(role);

                if (result.Succeeded)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedRole, role);
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.RoleExist, role);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Dashboard", model);
            }

            return this.RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInRole(DashboardIndexViewModel model)
        {
            string inputRole = model.AddUserInRole.Role;
            string inputUsername = model.AddUserInRole.Username;

            if (this.ModelState.IsValid)
            {
                var isAdded = await this.dashboardService.IsAddedUserInRole(inputRole, inputUsername);

                if (isAdded)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyAddedUserInRole, inputUsername.ToUpper(), inputRole);
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.UserAlreadyInRole, inputUsername.ToUpper(), inputRole);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Dashboard", model);
            }

            return this.RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(DashboardIndexViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var username = model.RemoveUserFromRole.Username;
                var role = model.RemoveUserFromRole.Role;
                bool isRemoved = await this.dashboardService.RemoveUserFromRole(username, role);

                if (isRemoved)
                {
                    this.TempData["Success"] = string.Format(SuccessMessages.SuccessfullyRemoveUserRole, username.ToUpper(), role);
                    return this.Redirect($"/Profile/{username}");
                }
                else
                {
                    this.TempData["Error"] = string.Format(ErrorMessages.UserNotInRole, username.ToUpper(), role);
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Dashboard", model);
            }

            return this.RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> SyncFollowUnfollow()
        {
            bool isSync = await this.dashboardService.SyncFollowUnfollow();

            if (isSync)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullySyncFollowUnfollow;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.NoDataForSyncFollowUnfollow;
            }

            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}