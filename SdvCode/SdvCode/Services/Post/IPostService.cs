// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Post.ViewModels;

    public interface IPostService
    {
        Task<bool> LikePost(string id);

        PostViewModel ExtractCurrentPost(string id);
    }
}