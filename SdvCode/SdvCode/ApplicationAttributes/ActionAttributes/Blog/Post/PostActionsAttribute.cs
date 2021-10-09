// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ApplicationAttributes.ActionAttributes.Blog.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Enums;

    public class PostActionsAttribute : BlogRoleAttribute
    {
        public PostActionsAttribute(string actionName, string controllerName, object routValues, string message)
            : base(actionName, controllerName)
        {
            this.RoutValues = routValues;
            this.Message = message;
        }

        public object RoutValues { get; }

        public string Message { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var db = context
                .HttpContext
                .RequestServices
                .GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            if (context.ActionArguments.ContainsKey("postId"))
            {
                var postId = context.ActionArguments["postId"].ToString();
                var post = db.Posts.FirstOrDefault(x => x.Id == postId);

                var controller = context.Controller as Controller;

                if (post == null)
                {
                    controller.TempData["Error"] = ErrorMessages.NotExistingPost;
                    context.Result = new RedirectToActionResult(
                        this.ActionName,
                        this.ControllerName,
                        this.RoutValues);
                }
                else if (post.PostStatus != PostStatus.Approved)
                {
                    controller.TempData["Error"] = this.Message;
                    context.Result = new RedirectToActionResult(
                        this.ActionName,
                        this.ControllerName,
                        this.RoutValues);
                }
            }
        }
    }
}