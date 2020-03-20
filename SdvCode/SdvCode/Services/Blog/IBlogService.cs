// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Post.InputModels;
    using SdvCode.ViewModels.Post.ViewModels;

    public interface IBlogService
    {
        Task<ICollection<string>> ExtractAllCategoryNames();

        Task<ICollection<string>> ExtractAllTagNames();

        Task<bool> CreatePost(CreatePostIndexModel model, HttpContext httpContext);

        Task<ICollection<PostViewModel>> ExtraxtAllPosts(HttpContext httpContext);

        Task<bool> DeletePost(string id, HttpContext httpContext);

        Task<EditPostInputModel> ExtractPost(string id, HttpContext httpContext);

        Task<bool> EditPost(EditPostInputModel model, HttpContext httpContext);
    }
}