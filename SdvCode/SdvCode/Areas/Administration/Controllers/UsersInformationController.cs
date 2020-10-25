// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Services.UsersInformation;
    using SdvCode.Areas.Administration.ViewModels.UsersInformation;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class UsersInformationController : Controller
    {
        private readonly IUsersInformationService usersInformation;

        public UsersInformationController(IUsersInformationService usersInformation)
        {
            this.usersInformation = usersInformation;
        }

        public async Task<IActionResult> AllUsers()
        {
            AllUsersViewModel model = await this.usersInformation.GetAllUsers();
            return this.View(model);
        }
    }
}