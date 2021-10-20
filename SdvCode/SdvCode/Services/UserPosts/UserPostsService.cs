// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;

    using X.PagedList;

    public class UserPostsService : IUserPostsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserPostsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<ICollection<BlogPostCardViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser currentUser)
        {
            Expression<Func<Post, bool>> postsFilter;
            var user = await this.userManager.FindByNameAsync(username);

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                postsFilter = x => (x.PostStatus == PostStatus.Banned || x.PostStatus == PostStatus.Pending || x.PostStatus == PostStatus.Approved) && x.ApplicationUser.UserName == user.UserName;
            }
            else
            {
                if (currentUser != null)
                {
                    postsFilter = x => x.PostStatus == PostStatus.Approved && x.ApplicationUserId == user.Id;
                }
                else
                {
                    postsFilter = x => x.PostStatus == PostStatus.Approved;
                }
            }

            var posts = this.db.Posts
                .Where(postsFilter)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .Include(x => x.FavouritePosts)
                .Include(x => x.PostLikes)
                .Include(x => x.PostsTags)
                .ThenInclude(x => x.Tag)
                .AsSplitQuery()
                .OrderByDescending(x => x.UpdatedOn)
                .ToList();

            var model = this.mapper.Map<List<BlogPostCardViewModel>>(posts);
            return model;
        }

        public async Task<ICollection<BlogPostCardViewModel>> ExtractLikedPostsByUsername(string username, ApplicationUser currentUser)
        {
            Expression<Func<Post, bool>> postsFilter;
            var user = await this.userManager.FindByNameAsync(username);

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                postsFilter = x => (x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending ||
                    x.PostStatus == PostStatus.Approved) && x.PostLikes.Any(y => y.UserId == user.Id && y.IsLiked);
            }
            else
            {
                if (currentUser != null)
                {
                    postsFilter = x => (x.PostStatus == PostStatus.Approved) && x.PostLikes.Any(y => y.UserId == user.Id && y.IsLiked);
                }
                else
                {
                    postsFilter = x => x.PostStatus == PostStatus.Approved;
                }
            }

            var posts = this.db.Posts
                .Where(postsFilter)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .Include(x => x.FavouritePosts)
                .Include(x => x.PostLikes)
                .Include(x => x.PostsTags)
                .ThenInclude(x => x.Tag)
                .AsSplitQuery()
                .OrderByDescending(x => x.UpdatedOn)
                .ToList();

            var model = this.mapper.Map<List<BlogPostCardViewModel>>(posts);
            return model;
        }
    }
}