// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;

    [Area(GlobalConstants.AdministrationArea)]
    public class PendingCommentsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}