// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;

    public interface IUserPostsService
    {
        Task<ICollection<BlogPostCardViewModel>> ExtractLikedPostsByUsername(string username, ApplicationUser user);

        Task<ICollection<BlogPostCardViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser user);
    }
}