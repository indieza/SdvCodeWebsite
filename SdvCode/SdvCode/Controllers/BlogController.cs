// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using Twilio.Rest.Api.V2010.Account.Usage;

    public class BlogController : Controller
    {
        private readonly IBlogService blogService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GlobalUserValidator userValidator;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            this.blogService = blogService;
            this.userManager = userManager;
            this.userValidator = new GlobalUserValidator(this.userManager);
        }

        public IActionResult Index()
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;
            var model = new BlogViewModel
            {
                Posts = this.blogService.ExtraxtAllPosts(user),
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult CreatePost()
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;

            var isBlocked = this.userValidator.IsBlocked(user);
            if (isBlocked == true)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = this.userValidator.IsInBlogRole(user);
            if (isInRole == false)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var model = new CreatePostIndexModel
            {
                Categories = this.blogService.ExtractAllCategoryNames(),
                Tags = this.blogService.ExtractAllTagNames(),
                PostInputModel = new CreatePostInputModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostIndexModel model)
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User).Result;

            if (this.ModelState.IsValid)
            {
                bool isAdded = await this.blogService.CreatePost(model, user);

                if (isAdded)
                {
                    this.TempData["Success"] = SuccessMessages.SuccessfullyCreatedPost;
                }
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Blog");
        }
    }
}