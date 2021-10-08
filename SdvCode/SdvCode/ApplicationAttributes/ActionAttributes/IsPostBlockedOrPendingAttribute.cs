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

    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;

    public class IsPostBlockedOrPendingAttribute : ActionFilterAttribute
    {
        private readonly string redirectActionName;
        private readonly string redirectControllerName;

        public IsPostBlockedOrPendingAttribute(string redirectActionName, string redirectControllerName)
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

            var postId = context.ActionArguments["id"].ToString();
            var post = db.Posts.FirstOrDefault(x => x.Id == postId);

            var controller = context.Controller as Controller;

            if (post == null)
            {
                controller.TempData["Error"] = "My Message";
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
            else if (post.PostStatus == PostStatus.Banned || post.PostStatus == PostStatus.Pending)
            {
                controller.TempData["Error"] = "My Message";
                context.Result = new RedirectToActionResult(
                    this.redirectActionName,
                    this.redirectControllerName,
                    null);
            }
        }
    }
}