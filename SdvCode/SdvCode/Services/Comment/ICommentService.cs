// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Comment
{
    using System;
    using System.Threading.Tasks;

    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Comment.ViewModels;

    public interface ICommentService
    {
        Task<Tuple<string, string>> Create(string postId, ApplicationUser user, string content, string parentId);

        bool IsInPostId(string parentId, string postId);

        Task<Tuple<string, string>> DeleteCommentById(string commentId);

        Task<Post> ExtractCurrentPost(string postId);

        Task<bool> IsParentCommentApproved(string parentId);

        Task<bool> IsCommentIdCorrect(string commentId, string postId);

        Task<EditCommentViewModel> ExtractCurrentComment(string commentId);

        Task<Tuple<string, string>> EditComment(EditCommentViewModel model);
    }
}