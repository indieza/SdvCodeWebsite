// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class AddChatStickerController : Controller
    {
        public AddChatStickerController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}