// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;

    [Authorize]
    [Area(GlobalConstants.PrivateChatArea)]
    public class CollectStickersController : Controller
    {
        public CollectStickersController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}