// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.ViewModels;

    public class BlogViewComponent : ViewComponent
    {
        private readonly IBlogComponentService blogComponentService;

        public BlogViewComponent(IBlogComponentService blogComponentService)
        {
            this.blogComponentService = blogComponentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string search)
        {
            BlogComponentViewModel components = new BlogComponentViewModel
            {
                RecentPosts = await this.blogComponentService.ExtractRecentPosts(this.HttpContext),
                TopCategories = await this.blogComponentService.ExtractTopCategories(),
                TopPosts = await this.blogComponentService.ExtractTopPosts(this.HttpContext),
                TopTags = await this.blogComponentService.ExtractTopTags(),
                Search = search,
            };

            return this.View(components);
        }
    }
}