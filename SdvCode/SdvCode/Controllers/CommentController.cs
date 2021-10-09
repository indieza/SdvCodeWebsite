// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.ApplicationAttributes.ActionAttributes;
    using SdvCode.ApplicationAttributes.ActionAttributes.Blog.Comment;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Comment;
    using SdvCode.ViewModels.Comment.InputModels;
    using SdvCode.ViewModels.Comment.ViewModels;

    [Authorize]
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

        [HttpPost]
        [UserBlocked("Index", "Profile")]
        [CommentCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToCreateComment)]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            if (this.ModelState.IsValid)
            {
                var parentId = input.ParentId == "0" ? null : input.ParentId;
                if (parentId != null)
                {
                    if (!this.commentsService.IsInPostId(parentId, input.PostId))
                    {
                        this.TempData["Error"] = ErrorMessages.DontMakeBullshits;
                        return this.RedirectToAction("Index", "Post", new { postId = input.PostId });
                    }

                    bool isParentApproved = await this.commentsService.IsParentCommentApproved(parentId);
                    if (!isParentApproved)
                    {
                        this.TempData["Error"] = ErrorMessages.CannotCommentNotApprovedComment;
                        return this.RedirectToAction("Index", "Post", new { postId = input.PostId });
                    }
                }

                var currentUser = await this.userManager.GetUserAsync(this.User);

                Post currentPost = await this.commentsService.ExtractCurrentPost(input.PostId);
                if (currentPost.PostStatus == PostStatus.Banned || currentPost.PostStatus == PostStatus.Pending)
                {
                    this.TempData["Error"] = ErrorMessages.CannotCommentNotApprovedBlogPost;
                    return this.RedirectToAction("Index", "Post", new { postId = input.PostId });
                }

                var tuple = await this.commentsService
                    .Create(input.PostId, currentUser, input.SanitizedContent, parentId);
                this.TempData[tuple.Item1] = tuple.Item2;

                return this.RedirectToAction("Index", "Post", new { postId = input.PostId });
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "Blog");
        }

        [HttpPost]
        [Route("/Comment/DeleteById/{commentId}/{postId}")]
        [UserBlocked("Index", "Profile")]
        [CommentCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToDeleteComment)]
        public async Task<IActionResult> DeleteById(string commentId, string postId)
        {
            var tuple = await this.commentsService.DeleteCommentById(commentId);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Post", new { postId });
        }

        [HttpGet]
        [Route("/Comment/EditComment/{commentId}/{postId}")]
        [UserBlocked("Index", "Profile")]
        [CommentCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToEditComment)]
        public async Task<IActionResult> EditComment(string commentId, string postId)
        {
            var isCommentIdCorrect = await this.commentsService.IsCommentIdCorrect(commentId, postId);

            if (!isCommentIdCorrect)
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Post", new { postId });
            }

            EditCommentViewModel model = await this.commentsService.ExtractCurrentComment(commentId);

            return this.View(model);
        }

        [HttpPost]
        [Route("/Comment/EditComment/{commentId}/{postId}")]
        [UserBlocked("Index", "Profile")]
        [CommentCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToEditComment)]
        public async Task<IActionResult> EditComment(string commentId, string postId, EditCommentViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var isCommentIdCorrect = await this.commentsService.IsCommentIdCorrect(commentId, postId);

                if (!isCommentIdCorrect)
                {
                    this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                    return this.RedirectToAction("Index", "Post", new { postId });
                }

                Tuple<string, string> tuple = await this.commentsService.EditComment(model);
                this.TempData[tuple.Item1] = tuple.Item2;

                return this.RedirectToAction("Index", "Post", new { postId });
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "Blog");
        }
    }
}