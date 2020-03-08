using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels;
using SdvCode.Services;

namespace SdvCode.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class DashboardController : Controller
    {
        private readonly IAdministrationService administrationService;
        private readonly RoleManager<IdentityRole> roleManager;

        public DashboardController(IAdministrationService administrationService, RoleManager<IdentityRole> roleManager)
        {
            this.administrationService = administrationService;
            this.roleManager = roleManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            DashboardViewModel dashboard = this.administrationService.GetDashboardInformation();
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
            if (this.ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.CreateRoleInputModel.Role
                };

                IdentityResult result = await this.roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Success added role.";
                    return RedirectToAction("Index", "Dashboard");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}