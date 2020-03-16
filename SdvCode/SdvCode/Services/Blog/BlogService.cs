// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.CloudServices;
    using SdvCode.ViewModels.Blog.InputModels;
    using SdvCode.ViewModels.Blog.ViewModels;

    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public BlogService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<bool> CreatePost(CreatePostIndexModel model, ApplicationUser user)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Name == model.PostInputModel.CategoryName);
            var imageName = string.Join(string.Empty, model.PostInputModel.Title.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            var imageUrl = await ApplicationCloudinary.UploadImage(
                this.cloudinary,
                model.PostInputModel.CoverImage,
                string.Format(
                    GlobalConstants.CloudinaryPostCoverImageName,
                    imageName));

            var post = new Post
            {
                Title = model.PostInputModel.Title,
                Category = category,
                Content = model.PostInputModel.Content,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ShortContent = $"{model.PostInputModel.Content.Substring(0, 347)}...",
                ApplicationUser = user,
                Likes = 0,
            };

            if (imageUrl != null)
            {
                post.ImageUrl = imageUrl;
            }

            foreach (var tagName in model.PostInputModel.TagsNames)
            {
                var tag = this.db.Tags.FirstOrDefault(x => x.Name.ToLower() == tagName.ToLower());
                post.PostsTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id,
                });
            }

            this.db.Posts.Add(post);
            this.db.UserActions.Add(new UserAction
            {
                Action = UserActionsType.CreatePost,
                ActionDate = DateTime.UtcNow,
                ApplicationUserId = user.Id,
                PostId = post.Id,
                PersonProfileImageUrl = user.ImageUrl,
                PersonUsername = user.UserName,
                ProfileImageUrl = user.ImageUrl,
            });
            await this.db.SaveChangesAsync();
            return true;
        }

        public ICollection<string> ExtractAllCategoryNames()
        {
            return this.db.Categories.Select(x => x.Name).ToList();
        }

        public ICollection<string> ExtractAllTagNames()
        {
            return this.db.Tags.Select(x => x.Name).ToList();
        }

        public ICollection<Post> ExtraxtAllPosts()
        {
            var posts = this.db.Posts.OrderByDescending(x => x.CreatedOn).ToList();

            foreach (var post in posts)
            {
                post.Category = this.db.Categories.FirstOrDefault(x => x.Id == post.CategoryId);
                post.ApplicationUser = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
            }

            return posts;
        }
    }
}