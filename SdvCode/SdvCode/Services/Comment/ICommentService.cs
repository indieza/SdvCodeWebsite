// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Comment
{
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public interface ICommentService
    {
        Task<bool> Create(string postId, ApplicationUser user, string content, string parentId);

        bool IsInPostId(string parentId, string postId);

        Task<bool> DeleteCommentById(string commentId);

        Task<Post> ExtractCurrentPost(string postId);

        Task<bool> IsParentCommentApproved(string parentId);
    }
}