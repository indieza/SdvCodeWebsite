// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Editor.Services.Comment;
    using SdvCode.Constraints;
    using SdvCode.Models.User;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class CommentController : Controller
    {
        private readonly IEditorCommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(IEditorCommentService commentService, UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> ApproveComment(string commentId, string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (currentUser.IsBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isApproved = await this.commentService.ApprovedCommentById(commentId, currentUser);

            if (isApproved)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyApprovedComment;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { postId = postId });
        }
    }
}