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

    public class IsUserInComentRoleAttribute : ActionFilterAttribute
    {
        private readonly string redirectActionName;
        private readonly string redirectControllerName;

        public IsUserInComentRoleAttribute(string redirectActionName, string redirectControllerName)
        {
            this.redirectActionName = redirectActionName;
            this.redirectControllerName = redirectControllerName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var db = context
                .HttpContext
                .RequestServices
                .GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            var userManager = context
                .HttpContext
                .RequestServices
                .GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var username = context.HttpContext.User.Identity.Name;
            var user = userManager.FindByNameAsync(username).Result;

            var commentId = context.ActionArguments["commentId"].ToString();
            var postId = context.ActionArguments["postId"].ToString();

            var comment = db.Comments.FirstOrDefault(x => x.Id == commentId);
            var userPostsIds = db.Posts.Where(x => x.ApplicationUserId == user.Id).Select(x => x.Id).ToList();

            var controller = context.Controller as Controller;

            if (comment == null)
            {
                controller.TempData["Error"] = ErrorMessages.NotExistingComment;
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    new { id = postId });
            }
            else if (!(userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                       userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result ||
                       comment.ApplicationUserId == user.Id))
            {
                controller.TempData["Error"] = ErrorMessages.NoPermissionToDeleteComment;
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    new { id = postId });
            }
        }
    }
}