// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
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

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IHomeService homeService;

        public HomeController(
            ILogger<HomeController> logger,
            IHomeService homeService)
        {
            this.logger = logger;
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                TotalRegisteredUsers = this.homeService.GetRegisteredUsersCount(),
                Administrators = this.homeService.GetAllAdministrators(),
            };

            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>().ToArray())
            {
                IdentityResult result = await this.homeService.CreateRole(role.ToString());
            }

            return this.View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}