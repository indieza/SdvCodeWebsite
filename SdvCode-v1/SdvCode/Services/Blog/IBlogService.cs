﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Post.InputModels;

    public interface IBlogService
    {
        Task<ICollection<string>> ExtractAllCategoryNames();

        Task<ICollection<string>> ExtractAllTagNames();

        Task<Tuple<string, string>> CreatePost(CreatePostIndexModel model, ApplicationUser user);

        Task<ICollection<BlogPostCardViewModel>> ExtraxtAllPosts(ApplicationUser user, string search);

        Task<Tuple<string, string>> DeletePost(string id, ApplicationUser user);

        Task<EditPostInputModel> ExtractPost(string id);

        Task<Tuple<string, string>> EditPost(EditPostInputModel model, ApplicationUser user);

        Task<bool> IsPostExist(string id);
    }
}