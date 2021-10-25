// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SdvCode.ApplicationAttributes.ActionAttributes;
    using SdvCode.ApplicationAttributes.ActionAttributes.Blog.Post;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Post.InputModels;

    using X.PagedList;

    public class BlogController : Controller
    {
        private readonly IBlogService blogService;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogController(
            IBlogService blogService,
            UserManager<ApplicationUser> userManager)
        {
            this.blogService = blogService;
            this.userManager = userManager;
        }

        /// <summary>
        /// This function will return a list of all Blog Posts.
        /// </summary>
        /// <param name="page">Current page number.</param>
        /// <param name="search">Current search text which will filter all Blog Posts.</param>
        /// <returns>Returns a view with a collection with all BLog Posts.</returns>
        [HttpGet]
        [Route("Blog/{page?}/{search?}")]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var pageNumber = page ?? 1;

            if (!string.IsNullOrEmpty(search.Trim()))
            {
                pageNumber = 1;
            }

            var posts = await this.blogService.ExtraxtAllPosts(currentUser, search);
            var model = new BlogViewModel
            {
                Posts = posts.ToPagedList(pageNumber, GlobalConstants.BlogPostsOnPage),
                Search = search.Trim(),
            };

            return this.View(model);
        }

        /// <summary>
        ///  This function will return a View with needed information for a Blog Post creation.
        /// </summary>
        /// <returns>Returns a view with data which is needed to create a Blog Post.</returns>
        [HttpGet]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionsToCreateBlogPost)]
        public async Task<IActionResult> CreatePost()
        {
            var model = new CreatePostIndexModel
            {
                Categories = await this.blogService.ExtractAllCategoryNames(),
                Tags = await this.blogService.ExtractAllTagNames(),
                PostInputModel = new CreatePostInputModel(),
            };

            return this.View(model);
        }

        /// <summary>
        /// This function will create a new Blog Post.
        /// </summary>
        /// <param name="model">Data Input Model for Blog Post Creation Data.</param>
        /// <returns>Redirect to Page based on IF-ELSE statement over the Input Model.</returns>
        [HttpPost]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionsToCreateBlogPost)]
        public async Task<IActionResult> CreatePost(CreatePostIndexModel model)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (this.ModelState.IsValid)
            {
                var tuple = await this.blogService.CreatePost(model, currentUser);
                this.TempData[tuple.Item1] = tuple.Item2;
                return this.RedirectToAction("Index", "Blog");
            }
            else
            {
                this.TempData["Error"] = ErrorMessages.InvalidInputModel;
                return this.RedirectToAction("Index", "Blog", model);
            }
        }

        /// <summary>
        /// This function will delete a Blog Post by its ID.
        /// </summary>
        /// <param name="postId">The target Blog Post ID.</param>
        /// <returns>Redirect to Page based on some IF-ELSE statements over the Input Model.</returns>
        [HttpPost]
        [Authorize]
        [Route("/Blog/DeletePost/{postId}")]
        [UserBlocked("Index", "Profile")]
        [PostCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionsToDeleteBlogPost)]
        public async Task<IActionResult> DeletePost(string postId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tuple = await this.blogService.DeletePost(postId, currentUser);
            this.TempData[tuple.Item1] = tuple.Item2;
            return this.RedirectToAction("Index", "Blog");
        }

        /// <summary>
        /// This function will extract a target Blog Post information.
        /// </summary>
        /// <param name="id">ID of the target Blog Post for editing.</param>
        /// <returns>Returns a View with a data for the target Blog Post.</returns>
        [HttpGet]
        [Route("/Blog/EditPost/{id}")]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToEditBlogPost)]
        public async Task<IActionResult> EditPost(string id)
        {
            if (!await this.blogService.IsPostExist(id))
            {
                return this.NotFound();
            }

            EditPostInputModel model = await this.blogService.ExtractPost(id);
            model.Categories = await this.blogService.ExtractAllCategoryNames();
            model.Tags = await this.blogService.ExtractAllTagNames();

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        [UserBlocked("Index", "Profile")]
        [PostCrudOperations("Index", "Blog", null, ErrorMessages.NoPermissionToEditBlogPost)]
        public async Task<IActionResult> EditPost(EditPostInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                var tuple = await this.blogService.EditPost(model, currentUser);
                this.TempData[tuple.Item1] = tuple.Item2;
                return this.RedirectToAction("Index", "Post", new { postId = model.Id });
            }

            this.TempData["Error"] = ErrorMessages.InvalidInputModel;
            return this.RedirectToAction("Index", "Blog");
        }
    }
}