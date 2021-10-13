// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.User;

    public interface IUserPostsService
    {
        Task<ICollection<PostViewModel>> ExtractLikedPostsByUsername(string username, ApplicationUser user);

        Task<ICollection<PostViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser user);
    }
}