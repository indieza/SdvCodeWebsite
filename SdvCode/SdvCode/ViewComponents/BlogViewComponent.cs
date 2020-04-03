// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.ViewModels;

    public class BlogViewComponent : ViewComponent
    {
        private readonly IBlogComponentService blogComponentService;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogViewComponent(IBlogComponentService blogComponentService, UserManager<ApplicationUser> userManager)
        {
            this.blogComponentService = blogComponentService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string search)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            BlogComponentViewModel components = new BlogComponentViewModel
            {
                RecentPosts = await this.blogComponentService.ExtractRecentPosts(currentUser),
                TopCategories = await this.blogComponentService.ExtractTopCategories(),
                TopPosts = await this.blogComponentService.ExtractTopPosts(currentUser),
                TopTags = await this.blogComponentService.ExtractTopTags(),
                RecentComments = await this.blogComponentService.ExtractRecentComments(currentUser),
                Search = search,
            };

            return this.View(components);
        }
    }
}