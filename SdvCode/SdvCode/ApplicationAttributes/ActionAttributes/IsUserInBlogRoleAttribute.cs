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

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class IsUserInBlogRoleAttribute : ActionFilterAttribute
    {
        private readonly string redirectActionName;
        private readonly string redirectControllerName;

        public IsUserInBlogRoleAttribute(string redirectActionName, string redirectControllerName)
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
            var user = userManager.FindByNameAsync(username).Result;
            var controller = context.Controller as Controller;

            if (!(userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                 userManager.IsInRoleAsync(user, Roles.Author.ToString()).Result ||
                 userManager.IsInRoleAsync(user, Roles.Contributor.ToString()).Result ||
                 userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result))
            {
                controller.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
        }
    }
}