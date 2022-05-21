// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes.ActionAttributes.Blog
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
    using SdvCode.Models.User;

    public class BlogRoleAttribute : ActionFilterAttribute
    {
        public BlogRoleAttribute(string actionName, string controllerName)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
        }

        public string ActionName { get; }

        public string ControllerName { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userManager = context
                .HttpContext
                .RequestServices
                .GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var username = context.HttpContext.User.Identity.Name;
            var user = userManager.FindByNameAsync(username).Result;
            var controller = context.Controller as Controller;

            var isInRole = userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result ||
                userManager.IsInRoleAsync(user, Roles.Author.ToString()).Result ||
                userManager.IsInRoleAsync(user, Roles.Contributor.ToString()).Result;

            if (!isInRole)
            {
                controller.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                context.Result = new RedirectToActionResult(
                    this.ActionName,
                    this.ControllerName,
                    null);
            }
        }
    }
}