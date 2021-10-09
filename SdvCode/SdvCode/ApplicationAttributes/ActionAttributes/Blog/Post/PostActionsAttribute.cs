using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using SdvCode.Constraints;
using SdvCode.Data;
using SdvCode.Models.Enums;

namespace SdvCode.ApplicationAttributes.ActionAttributes.Blog.Post
{
    public class PostActionsAttribute : BlogRoleAttribute
    {
        private readonly object routValues;
        private readonly string message;

        public PostActionsAttribute(string actionName, string controllerName, object routValues, string message)
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

            var postId = context.ActionArguments["id"]?.ToString();
            var post = db.Posts.FirstOrDefault(x => x.Id == postId);

            var controller = context.Controller as Controller;

            if (post == null)
            {
                controller.TempData["Error"] = ErrorMessages.NotExistingPost;
                context.Result = new RedirectToActionResult(
                    this.ActionName,
                    this.ControllerName,
                    this.routValues);
            }
            else if (post.PostStatus != PostStatus.Approved)
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