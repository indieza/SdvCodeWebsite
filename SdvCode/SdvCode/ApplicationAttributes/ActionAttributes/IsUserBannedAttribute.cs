// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes.ActionAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using SdvCode.Constraints;
    using SdvCode.Data;

    public class IsUserBannedAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public IsUserBannedAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.User.Identity.Name;
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);
            var controller = context.Controller as Controller;

            if (user.IsBlocked)
            {
                controller.TempData["Error"] = ErrorMessages.YouAreBlock;
                context.Result = new RedirectToActionResult("Index", "Blog", null);
            }
        }
    }
}