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
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class CanAccessBlogPostAttribute : ActionFilterAttribute
    {
        private readonly string redirectActionName;
        private readonly string redirectControllerName;
        private readonly string message;

        public CanAccessBlogPostAttribute(string redirectActionName, string redirectControllerName, string message)
        {
            this.redirectActionName = redirectActionName;
            this.redirectControllerName = redirectControllerName;
            this.message = message;
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

            var postId = context.ActionArguments["id"].ToString();
            var post = db.Posts.FirstOrDefault(x => x.Id == postId);

            var userPostsIds = db.Posts.Where(x => x.ApplicationUserId == user.Id).Select(x => x.Id).ToList();

            var controller = context.Controller as Controller;

            if (post == null)
            {
                controller.TempData["Error"] = ErrorMessages.NotExistingPost;
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
            else if (post.PostStatus == PostStatus.Banned &&
                     !userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result &&
                     !userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result)
            {
                controller.TempData["Error"] = this.message;
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
            else if (post.PostStatus != PostStatus.Approved && !userPostsIds.Contains(postId))
            {
                controller.TempData["Error"] = this.message;
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
        }
    }
}