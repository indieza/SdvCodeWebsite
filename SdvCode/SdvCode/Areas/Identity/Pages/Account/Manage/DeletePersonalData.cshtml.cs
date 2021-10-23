// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Cloud;
    using SdvCode.Services.Comment;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly ApplicationDbContext db;
        private readonly ICommentService commentService;
        private readonly Cloudinary cloudinary;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            ApplicationDbContext db,
            ICommentService commentService,
            Cloudinary cloudinary)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.db = db;
            this.commentService = commentService;
            this.cloudinary = cloudinary;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return this.Page();
                }
            }

            var comments = this.db.Comments.Where(x => x.ApplicationUserId == user.Id);
            foreach (var comment in comments)
            {
                await this.commentService.DeleteCommentById(comment.Id);
            }

            this.db.SaveChanges();

            var posts = this.db.Posts.Where(x => x.ApplicationUserId == user.Id).ToList();
            foreach (var post in posts)
            {
                var action = this.db.UserActions.Where(x => x.PostId == post.Id).ToList();
                var postComments = this.db.Comments.Where(x => x.PostId == post.Id).ToList();

                foreach (var comment in postComments)
                {
                    await this.commentService.DeleteCommentById(comment.Id);
                }

                this.db.UserActions.RemoveRange(action);
            }

            var followFollowed = this.db.FollowUnfollows
                .Where(x => x.ApplicationUserId == user.Id || x.FollowerId == user.Id)
                .ToList();

            var likes = this.db.PostsLikes
                .Where(x => x.UserId == user.Id)
                .ToList();
            var recommendedFriends = this.db.RecommendedFriends
                .Where(x => x.ApplicationUserId == user.Id || x.RecommendedApplicationUser.UserName == user.UserName)
                .ToList();
            var chatGroups = this.db.Groups
                .Where(x => x.Name.ToLower().Contains(user.UserName.ToLower()))
                .ToList();
            var chatMessages = this.db.ChatMessages
                .Where(x => x.ReceiverUsername == user.UserName || x.ApplicationUserId == user.Id)
                .ToList();

            foreach (var chatMessage in chatMessages)
            {
                var targetImages = this.db.ChatImages.Where(x => x.ChatMessageId == chatMessage.Id).ToList();

                foreach (var targetImage in targetImages)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        targetImage.Name,
                        GlobalConstants.PrivateChatImagesFolder);
                }

                this.db.ChatImages.RemoveRange(targetImages);
                this.db.SaveChanges();
            }

            var ratings = this.db.UserRatings
                .Where(x => x.RaterUsername == user.UserName || x.Username == user.UserName)
                .ToList();

            ApplicationCloudinary.DeleteImage(
                this.cloudinary,
                string.Format(GlobalConstants.CloudinaryUserProfilePictureName, user.UserName),
                string.Format(GlobalConstants.UserProfilePicturesFolder, user.UserName));
            ApplicationCloudinary.DeleteImage(
                this.cloudinary,
                string.Format(GlobalConstants.CloudinaryUserCoverImageName, user.UserName),
                string.Format(GlobalConstants.UserProfilePicturesFolder, user.UserName));

            foreach (var post in posts)
            {
                var allPostImages = this.db.PostImages
                       .Where(x => x.PostId == post.Id)
                       .ToList();

                foreach (var postImage in allPostImages)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.CloudinaryPostImageName, postImage.Id),
                        GlobalConstants.PostBaseImagesFolder);
                }

                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.CloudinaryPostCoverImageName, post.Id),
                    GlobalConstants.PostBaseImageFolder);
            }

            this.db.PostsLikes.RemoveRange(likes);
            this.db.FollowUnfollows.RemoveRange(followFollowed);
            this.db.Posts.RemoveRange(posts);
            this.db.RecommendedFriends.RemoveRange(recommendedFriends);
            this.db.Groups.RemoveRange(chatGroups);
            this.db.ChatMessages.RemoveRange(chatMessages);
            this.db.UserRatings.RemoveRange(ratings);
            await this.db.SaveChangesAsync();

            var favouriteStickerTypes = this.db.FavouriteStickers
                .Where(x => x.ApplicationUserId == user.Id)
                .ToList();

            this.db.FavouriteStickers.RemoveRange(favouriteStickerTypes);
            this.db.SaveChanges();

            var result = await this.userManager.DeleteAsync(user);
            var userId = await this.userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return this.Redirect("~/");
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}