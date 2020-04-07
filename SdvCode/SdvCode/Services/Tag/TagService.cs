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
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class TagService : GlobalPostsExtractor, ITagService
    {
        private readonly ApplicationDbContext db;

        public TagService(ApplicationDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<ICollection<PostViewModel>> ExtractPostsByTagId(string id, ApplicationUser user)
        {
            var postsIds = await this.db.PostsTags.Where(x => x.TagId == id).Select(x => x.PostId).ToListAsync();
            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(this.db.Posts.FirstOrDefault(x => x.Id == postId));
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