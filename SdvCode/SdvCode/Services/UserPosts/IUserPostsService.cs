// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;

    public interface IUserPostsService
    {
        Task<ICollection<Post>> ExtractLikedPostsByUsername(string username);

        Task<ICollection<Post>> ExtractCreatedPostsByUsername(string username);
    }
}