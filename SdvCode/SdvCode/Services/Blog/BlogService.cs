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

        public ICollection<RecentPostsViewModel> ExtractRecentPosts()
        {
            var posts = this.db.Posts.ToList().OrderByDescending(x => x.UpdatedOn).Take(3);
            var recentPosts = new List<RecentPostsViewModel>();

            foreach (var post in posts)
            {
                recentPosts.Add(new RecentPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = post.ApplicationUser,
                });
            }

            return recentPosts;
        }

        public ICollection<TopCategoriesViewModel> ExtractTopCategories()
        {
            var categories = this.db.Categories.ToList();
            var topCategories = new List<TopCategoriesViewModel>();

            foreach (var category in categories)
            {
                topCategories.Add(new TopCategoriesViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    PostsCount = this.db.Posts.Count(x => x.CategoryId == category.Id),
                });
            }

            return topCategories.OrderByDescending(x => x.PostsCount).Take(10).ToList();
        }

        public ICollection<TopPostsViewModel> ExtractTopPosts()
        {
            var posts = this.db.Posts.ToList().OrderByDescending(x => x.Comments.Count + x.Likes).Take(3);
            var topPosts = new List<TopPostsViewModel>();

            foreach (var post in posts)
            {
                topPosts.Add(new TopPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = post.ApplicationUser,
                });
            }

            return topPosts;
        }

        public ICollection<TopTagsViewModel> ExtractTopTags()
        {
            var tags = this.db.Tags.ToList();
            var topTags = new List<TopTagsViewModel>();

            foreach (var tag in tags)
            {
                topTags.Add(new TopTagsViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Count = this.db.PostsTags.Count(x => x.TagId == tag.Id),
                });
            }

            return topTags.OrderByDescending(x => x.Count).Take(10).ToList();
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