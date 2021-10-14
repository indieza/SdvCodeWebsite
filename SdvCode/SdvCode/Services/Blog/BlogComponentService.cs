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
                .AsSplitQuery()
                .Take(35)
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
                .AsSplitQuery()
                .Take(20)
                .ToList();

            var model = this.mapper.Map<List<RecentPostViewModel>>(posts);
            return model;
        }

        public List<TopCategoryViewModel> ExtractTopCategories()
        {
            var categories = this.db.Categories
                .Include(x => x.Posts)
                .AsSplitQuery()
                .Take(10)
                .ToList();

            var model = this.mapper.Map<List<TopCategoryViewModel>>(categories);
            return model;
        }

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
                .AsSplitQuery()
                .Take(10)
                .ToList();

            var model = this.mapper.Map<List<TopPostViewModel>>(posts);
            return model;
        }

        public List<TopTagViewModel> ExtractTopTags()
        {
            var tags = this.db.Tags
                .Include(x => x.TagsPosts)
                .AsSplitQuery()
                .Take(10)
                .ToList();

            var model = this.mapper.Map<List<TopTagViewModel>>(tags);
            return model;
        }
    }
}