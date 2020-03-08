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
        private readonly IDashboardService administrationService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public DashboardController(IDashboardService administrationService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.administrationService = administrationService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = db;
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
            string role = model.CreateRoleInputModel.Role;

            if (this.ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role
                };

                IdentityResult result = await this.roleManager.CreateAsync(identityRole);

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
            if (this.ModelState.IsValid)
            {
                ApplicationUser user = await this.userManager.FindByNameAsync(model.AddUserInRole.Username);
                IdentityRole role = await this.roleManager.FindByNameAsync(model.AddUserInRole.Role);

                if (role == null)
                {
                    TempData["Error"] =
                        $@"{user.UserName.ToUpper()} cannot added to non-existed role {model.AddUserInRole.Role}.
                                        Please, first add the role, then registered the user to that role";

                    return RedirectToAction("Index", "Dashboard");
                }

                var isExist = this.db.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == role.Id);

                if (!isExist)
                {
                    this.db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });

                    await this.db.SaveChangesAsync();

                    TempData["Success"] = $"{user.UserName.ToUpper()} is added in role {role.Name} successfully.";
                }
                else
                {
                    TempData["Error"] = $"{user.UserName.ToUpper()} is already in role {role.Name}.";
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