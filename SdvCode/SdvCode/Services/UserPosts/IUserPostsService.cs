// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public interface IUserPostsService
    {
        Task<ICollection<PostViewModel>> ExtractLikedPostsByUsername(string username, ApplicationUser user);

        Task<ICollection<PostViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser user);
    }
}