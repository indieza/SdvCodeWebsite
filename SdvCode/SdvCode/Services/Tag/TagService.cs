// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting.Internal;
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class TagService : GlobalPostsExtractor, ITagService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public TagService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
            : base(db)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByTagId(string id, ApplicationUser user)
        {
            var postsIds = this.db.PostsTags.Where(x => x.TagId == id).Select(x => x.PostId).ToList();
            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(this.db.Posts.FirstOrDefault(x => x.Id == postId));
            }

            if (user != null &&
                (await this.userManager.IsInRoleAsync(user, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
            {
                posts = posts
                    .OrderByDescending(x => x.Comments.Count + x.Likes)
                    .ToList();
            }
            else
            {
                if (user != null)
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved ||
                        x.ApplicationUserId == user.Id)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
                else
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .OrderByDescending(x => x.Comments.Count + x.Likes)
                        .ToList();
                }
            }

            List<PostViewModel> postsModel = await this.ExtractPosts(user, posts);

            return postsModel;
        }

        public async Task<Tag> ExtractTagById(string id)
        {
            return await this.db.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}