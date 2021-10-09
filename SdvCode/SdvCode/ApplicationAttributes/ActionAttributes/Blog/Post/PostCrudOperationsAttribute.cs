// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes.ActionAttributes.Blog.Post
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

    public class PostCrudOperationsAttribute : BlogRoleAttribute
    {
        private readonly object routValues;
        private readonly string message;

        public PostCrudOperationsAttribute(string actionName, string controllerName, object routValues, string message)
            : base(actionName, controllerName)
        {
            this.routValues = routValues;
            this.message = message;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

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

            if (context.ActionArguments.ContainsKey("id"))
            {
                var postId = context.ActionArguments["id"]?.ToString();
                var post = db.Posts.FirstOrDefault(x => x.Id == postId);
                var userPostsIds = db.Posts.Where(x => x.ApplicationUserId == user.Id).Select(x => x.Id).ToList();

                var controller = context.Controller as Controller;

                var isOwnPost = userPostsIds.Contains(postId);
                var hasFullControl = userManager.IsInRoleAsync(user, Roles.Administrator.ToString()).Result ||
                    userManager.IsInRoleAsync(user, Roles.Editor.ToString()).Result;

                if (post == null)
                {
                    controller.TempData["Error"] = ErrorMessages.NotExistingPost;
                    context.Result = new RedirectToActionResult(
                        this.ActionName,
                        this.ControllerName,
                        this.routValues);
                }
                else if (!isOwnPost && !hasFullControl)
                {
                    controller.TempData["Error"] = this.message;
                    context.Result = new RedirectToActionResult(
                        this.ActionName,
                        this.ControllerName,
                        this.routValues);
                }
            }
        }
    }
}