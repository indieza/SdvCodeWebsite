// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    using SdvCode.Constraints;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.UserPosts;
    using SdvCode.ViewModels.UserPosts;

    using X.PagedList;

    public class UserPostsController : Controller
    {
        private readonly IUserPostsService userPostsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPostsController(IUserPostsService userPostsService, UserManager<ApplicationUser> userManager)
        {
            this.userPostsService = userPostsService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("Blog/UserPosts/{username}/{filter}/{page?}")]
        public async Task<IActionResult> Index(string username, string filter, int? page)
        {
            UserPostsViewModel model = new UserPostsViewModel
            {
                Username = username,
            };

            var pageNumber = page ?? 1;

            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (filter == UserPostsFilter.Liked.ToString())
            {
                model.Action = UserPostsFilter.Liked.ToString();
                var posts = await this.userPostsService.ExtractLikedPostsByUsername(username, currentUser);
                model.Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage);
            }
            else if (filter == UserPostsFilter.Created.ToString())
            {
                model.Action = UserPostsFilter.Created.ToString();
                var posts = await this.userPostsService.ExtractCreatedPostsByUsername(username, currentUser);
                model.Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage);
            }

            return this.View(model);
        }
    }
}