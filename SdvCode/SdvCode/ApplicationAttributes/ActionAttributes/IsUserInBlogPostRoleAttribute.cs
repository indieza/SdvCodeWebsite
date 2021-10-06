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

    public class IsUserInBlogPostRoleAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public IsUserInBlogPostRoleAttribute(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var username = context.HttpContext.User.Identity.Name;
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);
            var controller = context.Controller as Controller;

            if (!(await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Author.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Contributor.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                controller.TempData["Error"] = ErrorMessages.NotInDeletePostRoles;
                context.Result = new RedirectToActionResult("Index", "Blog", null);
            }
        }
    }
}