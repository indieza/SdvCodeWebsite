// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes.ActionAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class UserBlockedAttribute : ActionFilterAttribute
    {
        private readonly string redirectActionName;
        private readonly string redirectControllerName;

        public UserBlockedAttribute(string redirectActionName, string redirectControllerName)
        {
            this.redirectActionName = redirectActionName;
            this.redirectControllerName = redirectControllerName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userManager = context
                .HttpContext
                .RequestServices
                .GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var username = context.HttpContext.User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                var user = userManager.FindByNameAsync(username).Result;
                var controller = context.Controller as Controller;

                if (user.IsBlocked)
                {
                    controller.TempData["Error"] = ErrorMessages.YouAreBlock;
                    context.Result = new RedirectToActionResult(
                        this.redirectActionName,
                        this.redirectControllerName,
                        new { username });
                }
            }
        }
    }
}