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
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.Services.Post;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Post.InputModels;
    using Twilio.Rest.Api.V2010.Account.Usage;
    using X.PagedList;

    public class BlogController : Controller
    {
        private readonly IBlogService blogService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly ApplicationDbContext db;
        private readonly GlobalUserValidator userValidator;

        public BlogController(
            IBlogService blogService,
            UserManager<ApplicationUser> userManager,
            IPostService postService,
            ApplicationDbContext db)
        {
            this.blogService = blogService;
            this.userManager = userManager;
            this.postService = postService;
            this.db = db;
            this.userValidator = new GlobalUserValidator(this.userManager, this.db);
        }

        [Route("Blog/{page?}/{search?}")]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var pageNumber = page ?? 1;
            var posts = await this.blogService.ExtraxtAllPosts(this.HttpContext, search);
            var model = new BlogViewModel
            {
                Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage),
                Search = search,
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> CreatePost()
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInBlogRole(this.HttpContext);
            if (!isInRole)
            {
                this.TempData["Error"] = string.Format(ErrorMessages.NotInBlogRoles, Roles.Contributor);
                return this.RedirectToAction("Index", "Blog");
            }

            var model = new CreatePostIndexModel
            {
                Categories = await this.blogService.ExtractAllCategoryNames(),
                Tags = await this.blogService.ExtractAllTagNames(),
                PostInputModel = new CreatePostInputModel(),
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> DeletePost(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInPostRole(this.HttpContext, id);
            if (isInRole == false)
            {
                this.TempData["Error"] = ErrorMessages.NotInDeletePostRoles;
                return this.RedirectToAction("Index", "Blog");
            }

            var isDeleted = await this.blogService.DeletePost(id, this.HttpContext);
            if (isDeleted == true)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyDeletePost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Blog");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostIndexModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool isAdded = await this.blogService.CreatePost(model, this.HttpContext);

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

        [Authorize]
        public async Task<IActionResult> EditPost(string id)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            var isApproved = await this.userValidator.IsPostBlocked(id, this.HttpContext);
            if (isApproved)
            {
                this.TempData["Error"] = ErrorMessages.CannotEditBlogPost;
                return this.RedirectToAction("Index", "Blog");
            }

            var isInRole = await this.userValidator.IsInPostRole(this.HttpContext, id);
            if (!isInRole)
            {
                this.TempData["Error"] = ErrorMessages.NotInEditPostRoles;
                return this.RedirectToAction("Index", "Blog");
            }

            EditPostInputModel model = await this.blogService.ExtractPost(id, this.HttpContext);
            model.Categories = await this.blogService.ExtractAllCategoryNames();
            model.Tags = await this.blogService.ExtractAllTagNames();

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost(EditPostInputModel model)
        {
            var isBlocked = await this.userValidator.IsBlocked(this.HttpContext);
            if (isBlocked)
            {
                this.TempData["Error"] = ErrorMessages.YouAreBlock;
                return this.RedirectToAction("Index", "Blog");
            }

            bool isEdited = await this.blogService.EditPost(model, this.HttpContext);

            if (isEdited)
            {
                this.TempData["Success"] = SuccessMessages.SuccessfullyEditedPost;
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            }

            return this.RedirectToAction("Index", "Post", new { model.Id });
        }
    }
}