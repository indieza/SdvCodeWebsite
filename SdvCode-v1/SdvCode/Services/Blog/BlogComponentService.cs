// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels;
    using SdvCode.ViewModels.Category.ViewModels.TopCategory;
    using SdvCode.ViewModels.Comment.ViewModels.RecentComment;
    using SdvCode.ViewModels.Post.ViewModels.RecentPost;
    using SdvCode.ViewModels.Post.ViewModels.TopPost;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Tag.TopTag;

    using X.PagedList;

    /// <summary>
    /// This service contains the logic to get all Blog information for Blog Component.
    /// </summary>
    public class BlogComponentService : IBlogComponentService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public BlogComponentService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// This function will return the most recent comments made in the Blog Part of the application.
        /// It contains filter logic relate to the current website visitor. Is it a visitor, a creator or user with higher priority roles.
        /// </summary>
        /// <param name="currentUser">Current logged in user.</param>
        /// <returns>Returns a Collection of Recent Comments.</returns>
        public async Task<ICollection<RecentCommentViewModel>> ExtractRecentComments(ApplicationUser currentUser)
        {
            Expression<Func<Comment, bool>> filterFunction;

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                filterFunction = x => x.CommentStatus == CommentStatus.Pending || x.CommentStatus == CommentStatus.Approved;
            }
            else
            {
                if (currentUser != null)
                {
                    filterFunction = x => x.CommentStatus == CommentStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id;
                }
                else
                {
                    filterFunction = x => x.CommentStatus == CommentStatus.Approved;
                }
            }

            var comments = this.db.Comments
                .Include(x => x.ApplicationUser)
                .Where(filterFunction)
                .OrderByDescending(x => x.UpdatedOn)
                .Take(GlobalConstants.RecentCommentsCount)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<RecentCommentViewModel>>(comments);

            foreach (var comment in model)
            {
                var contentWithoutTags = Regex.Replace(comment.Content, "<.*?>", string.Empty);

                comment.ShortContent = contentWithoutTags.Length < 95 ?
                        contentWithoutTags :
                        $"{contentWithoutTags.Substring(0, 95)}...";
            }

            return model;
        }

        /// <summary>
        /// This function will return the most recent posts made in the Blog Part of the application.
        /// It contains filter logic relate to the current website visitor. Is it a visitor, a creator or user with higher priority roles.
        /// </summary>
        /// <param name="currentUser">Current logged in user.</param>
        /// <returns>Returns a Collection of Recent Posts.</returns>
        public async Task<List<RecentPostViewModel>> ExtractRecentPosts(ApplicationUser currentUser)
        {
            Expression<Func<Post, bool>> filterFunction;

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                filterFunction = x => x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending ||
                    x.PostStatus == PostStatus.Approved;
            }
            else
            {
                if (currentUser != null)
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id;
                }
                else
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved;
                }
            }

            var posts = this.db.Posts
                .Include(x => x.ApplicationUser)
                .Where(filterFunction)
                .OrderByDescending(x => x.UpdatedOn)
                .Take(GlobalConstants.RecentPostsCount)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<RecentPostViewModel>>(posts);
            return model;
        }

        /// <summary>
        /// This function will return TOP Categories in the Blog Part of the application.
        /// </summary>
        /// <returns>Returns a Collection of TOP Categories.</returns>
        public List<TopCategoryViewModel> ExtractTopCategories()
        {
            var categories = this.db.Categories
                .Include(x => x.Posts)
                .Take(GlobalConstants.TopCategoriesCount)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<TopCategoryViewModel>>(categories);
            return model;
        }

        /// <summary>
        /// This function will return TOP Posts in the Blog Part of the application.
        /// </summary>
        /// <returns>Returns a Collection of TOP Posts.</returns>
        public async Task<List<TopPostViewModel>> ExtractTopPosts(ApplicationUser currentUser)
        {
            Expression<Func<Post, bool>> filterFunction;

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                filterFunction = x => x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending ||
                    x.PostStatus == PostStatus.Approved;
            }
            else
            {
                if (currentUser != null)
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == currentUser.Id;
                }
                else
                {
                    filterFunction = x => x.PostStatus == PostStatus.Approved;
                }
            }

            var posts = this.db.Posts
                .Include(x => x.ApplicationUser)
                .Where(filterFunction)
                .OrderByDescending(x => x.Comments.Count + x.Likes)
                .Take(GlobalConstants.TopPostsCount)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<TopPostViewModel>>(posts);
            return model;
        }

        /// <summary>
        /// This function will return TOP Tags in the Blog Part of the application.
        /// </summary>
        /// <returns>Returns a Collection of TOP Tags.</returns>
        public List<TopTagViewModel> ExtractTopTags()
        {
            var tags = this.db.Tags
                .Include(x => x.TagsPosts)
                .Take(GlobalConstants.TopTagsCount)
                .AsSplitQuery()
                .ToList();

            var model = this.mapper.Map<List<TopTagViewModel>>(tags);
            return model;
        }
    }
}