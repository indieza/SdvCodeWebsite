// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Editor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Areas.Editor.Services.Comment;
    using SdvCode.Constraints;

    [Authorize(Roles = GlobalConstants.EditorRole + "," + GlobalConstants.AdministratorRole)]
    [Area(GlobalConstants.EditorArea)]
    public class CommentController : Controller
    {
        private readonly IEditorCommentService commentService;

        public CommentController(IEditorCommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> ApproveComment(string commentId, string postId)
        {
            bool isApproved = await this.commentService.ApprovedCommentById(commentId);

            if (isApproved)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyApprovedComment;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { id = postId });
        }
    }
}