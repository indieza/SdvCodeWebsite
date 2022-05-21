﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Post
{
    using System;
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Post.ViewModels.PostPage;

    public interface IPostService
    {
        Task<Tuple<string, string>> LikePost(string id, ApplicationUser user);

        Task<PostViewModel> ExtractCurrentPost(string postId, ApplicationUser user);

        Task<Tuple<string, string>> UnlikePost(string id, ApplicationUser user);

        Task<Tuple<string, string>> AddToFavorite(ApplicationUser user, string id);

        Task<Tuple<string, string>> RemoveFromFavorite(ApplicationUser user, string id);

        Task<bool> IsPostExist(string id);
    }
}