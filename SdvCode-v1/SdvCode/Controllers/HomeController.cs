﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Models;
    using SdvCode.Services.Home;
    using SdvCode.ViewModels.Home;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>().ToArray())
            {
                _ = await this.homeService.CreateRole(role.ToString());
            }

            HomeViewModel model = new HomeViewModel
            {
                TotalRegisteredUsers = this.homeService.GetRegisteredUsersCount(),
                TotalBlogPosts = this.homeService.GetPostsCount(),
                TotalShopProducts = this.homeService.GetPorductsCount(),
                Administrators = await this.homeService.GetAllAdministrators(),
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult GetLatestBlogPosts()
        {
            ICollection<HomeLatestPostViewModel> latestPosts = this.homeService.GetLatestPosts();
            return new JsonResult(latestPosts);
        }

        [HttpGet]
        public async Task<IActionResult> GetHolidayTheme()
        {
            ICollection<string> icons = await this.homeService.GetHolidayThemeIcons();
            return new JsonResult(icons);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}