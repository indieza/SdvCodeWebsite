// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Tag
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
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Blog.ViewModels.BlogPostCard;
    using SdvCode.ViewModels.Tag;
    using SdvCode.ViewModels.Tag.TagPage;

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public TagService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<ICollection<BlogPostCardViewModel>> ExtractPostsByTagId(string id, ApplicationUser user)
        {
            Expression<Func<Post, bool>> postsFilter;

            if (user != null &&
                (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                postsFilter = x => (x.PostStatus == PostStatus.Approved ||
                    x.PostStatus == PostStatus.Banned ||
                    x.PostStatus == PostStatus.Pending) && x.PostsTags.Any(y => y.TagId == id);
            }
            else
            {
                if (user != null)
                {
                    postsFilter = x => (x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == user.Id) && x.PostsTags.Any(y => y.TagId == id);
                }
                else
                {
                    postsFilter = x => x.PostStatus == PostStatus.Approved && x.PostsTags.Any(y => y.TagId == id);
                }
            }

            var posts = this.db.Posts
                .Include(x => x.PostsTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .Include(x => x.FavouritePosts)
                .Include(x => x.PostLikes)
                .AsSplitQuery()
                .Where(postsFilter)
                .OrderByDescending(x => x.Comments.Count + x.Likes)
                .ToList();

            var model = this.mapper.Map<List<BlogPostCardViewModel>>(posts);
            return model;
        }

        public async Task<TagPageTagViewModel> ExtractTagById(string id)
        {
            var tag = await this.db.Tags.FirstOrDefaultAsync(x => x.Id == id);
            var model = this.mapper.Map<TagPageTagViewModel>(tag);
            return model;
        }
    }
}