// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Comment;
    using SdvCode.ViewModels.Comment;

    public class CommentController : Controller
    {
        private readonly ICommentService commentsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly GlobalUserValidator userValidator;

        public CommentController(
            ICommentService commentsService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
            this.db = db;
            this.userValidator = new GlobalUserValidator(this.userManager, this.db);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var parentId = input.ParentId == "0" ? null : input.ParentId;
            if (parentId != null)
            {
                if (!this.commentsService.IsInPostId(parentId, input.PostId))
                {
                    this.TempData["Error"] = ErrorMessages.DontMakeBullshits;
                    return this.RedirectToAction("Index", "Post", new { id = input.PostId });
                }

                bool isParentApproved = await this.commentsService.IsParentCommentApproved(parentId);
                if (!isParentApproved)
                {
                    this.TempData["Error"] = ErrorMessages.CannotCommentNotApprovedComment;
                    return this.RedirectToAction("Index", "Post", new { id = input.PostId });
                }
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isBlocked = this.userValidator.IsBlocked(currentUser);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Post", new { id = input.PostId });
            }

            var isInRole = await this.userValidator.IsInBlogRole(currentUser);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Post", new { id = input.PostId });
            }

            Post currentPost = await this.commentsService.ExtractCurrentPost(input.PostId);
            if (currentPost.PostStatus == PostStatus.Banned || currentPost.PostStatus == PostStatus.Pending)
            {
                this.TempData["Error"] = ErrorMessages.CannotCommentNotApprovedBlogPost;
                return this.RedirectToAction("Index", "Post", new { id = input.PostId });
            }

            bool isCreated = await this.commentsService
                .Create(input.PostId, currentUser, input.SanitizedContent, parentId);

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
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isInCommentRole = await this.userValidator.IsInCommentRole(currentUser, commentId);

            if (!isInCommentRole)
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

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