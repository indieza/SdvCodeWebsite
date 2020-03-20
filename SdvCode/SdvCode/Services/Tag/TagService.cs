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
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GlobalPostsExtractor postExtractor;

        public TagService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.postExtractor = new GlobalPostsExtractor(this.db);
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByTagId(string id, HttpContext httpContext)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
            var postsIds = await this.db.PostsTags.Where(x => x.TagId == id).Select(x => x.PostId).ToListAsync();
            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(this.db.Posts.FirstOrDefault(x => x.Id == postId));
            }

            List<PostViewModel> postsModel = await this.postExtractor.ExtractPosts(user, posts);

            return postsModel;
        }

        public async Task<Tag> ExtractTagById(string id)
        {
            return await this.db.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}