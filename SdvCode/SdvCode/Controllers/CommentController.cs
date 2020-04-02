namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using SdvCode.Services.Comment;
    using SdvCode.ViewModels.Comment;

    public class CommentController : Controller
    {
        private readonly ICommentService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(
            ICommentService commentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var parentId =
                input.ParentId == "0" ?
                    null :
                    input.ParentId;

            if (parentId != null)
            {
                if (!this.commentsService.IsInPostId(parentId, input.PostId))
                {
                    this.TempData["Error"] = ErrorMessages.DontMakeBullshits;
                    return this.RedirectToAction("Index", "Post", new { id = input.PostId });
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            bool isCreated = await this.commentsService
                .Create(input.PostId, userId, input.SanitizedContent, parentId);

            if (isCreated)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyAddedPostComment;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id = input.PostId });
        }

        public async Task<IActionResult> DeleteById(string commentId, string postId)
        {
            bool isDeleted = await this.commentsService.DeleteCommentById(commentId);

            if (isDeleted)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyDeletePostComment;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id = postId });
        }
    }
}