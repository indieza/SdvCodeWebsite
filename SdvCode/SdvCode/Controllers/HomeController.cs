using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Models;
using SdvCode.Services;
using SdvCode.ViewModels.Home;

namespace SdvCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService homeService;

        public HomeController(ILogger<HomeController> logger,
            IHomeService homeService)
        {
            _logger = logger;
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                TotalRegisteredUsers = this.homeService.GetRegisteredUsersCount()
            };

            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>().ToArray())
            {
                IdentityResult result = await this.homeService.CreateRole(role.ToString());
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}