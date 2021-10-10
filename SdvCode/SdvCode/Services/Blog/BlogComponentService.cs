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
                    .Include(x => x.ApplicationUser)
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    comments = this.db.Comments
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.CommentStatus == CommentStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .ToList();
                }
                else
                {
                    comments = this.db.Comments
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.CommentStatus == CommentStatus.Approved)
                        .OrderByDescending(x => x.UpdatedOn)
                        .ToList();
                }
            }

            var recentComments = new List<RecentCommentsViewModel>();

            foreach (var comment in comments.Take(35))
            {
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);
                recentComments.Add(new RecentCommentsViewModel
                {
                    User = comment.ApplicationUser,
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

        public List<RecentPostsViewModel> ExtractRecentPosts(ApplicationUser currentUser)
        {
            List<Post> posts = new List<Post>();

            if (currentUser != null &&
                (this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()).GetAwaiter().GetResult() ||
                this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString()).GetAwaiter().GetResult()))
            {
                posts = this.db.Posts
                    .Include(x => x.ApplicationUser)
                    .OrderByDescending(x => x.UpdatedOn)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = this.db.Posts
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .ToList();
                }
                else
                {
                    posts = this.db.Posts
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .ToList();
                }
            }

            return posts
                .Select(x => new RecentPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedOn = x.CreatedOn,
                    ImageUrl = x.ImageUrl,
                    ApplicationUser = x.ApplicationUser,
                    PostStatus = x.PostStatus,
                })
                .Take(20)
                .ToList();
        }

        public List<TopCategoriesViewModel> ExtractTopCategories()
        {
            return this.db.Categories
                .Include(x => x.Posts)
                .Select(x => new TopCategoriesViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PostsCount = x.Posts.Count(),
                })
                .Take(10)
                .ToList();
        }

        public async Task<List<TopPostsViewModel>> ExtractTopPosts(ApplicationUser currentUser)
        {
            List<Post> posts = new List<Post>();

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = this.db.Posts
                    .Include(x => x.ApplicationUser)
                    .OrderByDescending(x => x.Comments.Count + x.Likes)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = this.db.Posts
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
                else
                {
                    posts = this.db.Posts
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
            }

            return posts
                .Select(x => new TopPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedOn = x.CreatedOn,
                    ImageUrl = x.ImageUrl,
                    ApplicationUser = x.ApplicationUser,
                    PostStatus = x.PostStatus,
                })
                .Take(10)
                .ToList();
        }

        public List<TopTagsViewModel> ExtractTopTags()
        {
            return this.db.Tags
                .Include(x => x.TagsPosts)
                .Select(x => new TopTagsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.TagsPosts.Count(),
                })
                .Take(10)
                .ToList();
        }
    }
}