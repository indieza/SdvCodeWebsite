// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels;
    using X.PagedList;

    public class BlogComponentService : IBlogComponentService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogComponentService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<ICollection<RecentCommentsViewModel>> ExtractRecentComments(ApplicationUser currentUser)
        {
            List<Comment> comments = new List<Comment>();
            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                comments = this.db.Comments
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    comments = this.db.Comments
                        .Where(x => x.CommentStatus == CommentStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .ToList();
                }
                else
                {
                    comments = this.db.Comments
                        .Where(x => x.CommentStatus == CommentStatus.Approved)
                        .OrderByDescending(x => x.UpdatedOn)
                        .ToList();
                }
            }

            var recentComments = new List<RecentCommentsViewModel>();

            foreach (var comment in comments.Take(35))
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == comment.ApplicationUserId);
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);
                recentComments.Add(new RecentCommentsViewModel
                {
                    User = user,
                    CreatedOn = comment.CreatedOn,
                    CommentStatus = comment.CommentStatus,
                    ShortContent = contentWithoutTags.Length < 95 ?
                        contentWithoutTags :
                        $"{contentWithoutTags.Substring(0, 95)}...",
                    PostId = comment.PostId,
                });
            }

            return recentComments;
        }

        public async Task<List<RecentPostsViewModel>> ExtractRecentPosts(ApplicationUser currentUser)
        {
            List<Post> posts = new List<Post>();
            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = this.db.Posts
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = this.db.Posts
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .ToList();
                }
                else
                {
                    posts = this.db.Posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .ToList();
                }
            }

            var recentPosts = new List<RecentPostsViewModel>();

            foreach (var post in posts.Take(20))
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                recentPosts.Add(new RecentPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                    PostStatus = post.PostStatus,
                });
            }

            return recentPosts;
        }

        public async Task<List<TopCategoriesViewModel>> ExtractTopCategories()
        {
            var categories = this.db.Categories.ToList();
            var topCategories = new List<TopCategoriesViewModel>();

            foreach (var category in categories.Take(10))
            {
                topCategories.Add(new TopCategoriesViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    PostsCount = await this.db.Posts.CountAsync(x => x.CategoryId == category.Id),
                });
            }

            return topCategories.OrderByDescending(x => x.PostsCount).ToList();
        }

        public async Task<List<TopPostsViewModel>> ExtractTopPosts(ApplicationUser currentUser)
        {
            List<Post> posts = new List<Post>();

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = this.db.Posts
                    .OrderByDescending(x => x.Comments.Count + x.Likes)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = this.db.Posts
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
                else
                {
                    posts = this.db.Posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
            }

            var topPosts = new List<TopPostsViewModel>();

            foreach (var post in posts.Take(10))
            {
                var user = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);
                topPosts.Add(new TopPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    CreatedOn = post.CreatedOn,
                    ImageUrl = post.ImageUrl,
                    ApplicationUser = user,
                    PostStatus = post.PostStatus,
                });
            }

            return topPosts;
        }

        public async Task<List<TopTagsViewModel>> ExtractTopTags()
        {
            var tags = this.db.Tags.ToList();
            var topTags = new List<TopTagsViewModel>();

            foreach (var tag in tags.Take(10))
            {
                topTags.Add(new TopTagsViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Count = await this.db.PostsTags.CountAsync(x => x.TagId == tag.Id),
                });
            }

            return topTags.OrderByDescending(x => x.Count).ToList();
        }
    }
}