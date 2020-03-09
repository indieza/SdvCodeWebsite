using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels;
using SdvCode.Data;
using SdvCode.Models;
using SdvCode.Services;

namespace SdvCode.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [Authorize]
        public IActionResult Index()
        {
            DashboardViewModel dashboard = this.dashboardService.GetDashboardInformation();
            DashboardIndexViewModel model = new DashboardIndexViewModel
            {
                DashboardViewModel = dashboard,
                CreateRoleInputModel = new CreateRoleInputModel()
            };

            return View(model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateRole(DashboardIndexViewModel model)
        {
            string role = model.CreateRoleInputModel.Role;

            if (this.ModelState.IsValid)
            {
                IdentityResult result = await this.dashboardService.CreateRole(role);

                if (result.Succeeded)
                {
                    TempData["Success"] = $"Success added {role} role.";
                }
                else
                {
                    TempData["Error"] = $"{role} role already exits.";
                }
            }
            else
            {
                TempData["Error"] = "Unexpected error :( Maybe invalid Input Model.";
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> AddUserInRole(DashboardIndexViewModel model)
        {
            string inputRole = model.AddUserInRole.Role;
            string inputUsername = model.AddUserInRole.Username;

            if (this.ModelState.IsValid)
            {
                var isAdded = await this.dashboardService.IsAddedUserInRole(inputRole, inputUsername);

                if (isAdded)
                {
                    TempData["Success"] = $"{inputUsername.ToUpper()} is added in role {inputRole} successfully.";
                }
                else
                {
                    TempData["Error"] = $"{inputUsername.ToUpper()} is already in role {inputRole}.";
                }
            }
            else
            {
                TempData["Error"] = "Unexpected error :( Maybe invalid Input Model.";
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}