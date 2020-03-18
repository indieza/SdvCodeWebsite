// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext db;

        public TagService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<Post>> ExtractPostsByTagId(string id, ApplicationUser user)
        {
            var postsIds = await this.db.PostsTags.Where(x => x.TagId == id).Select(x => x.PostId).ToListAsync();
            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(this.db.Posts.FirstOrDefault(x => x.Id == postId));
            }

            foreach (var post in posts)
            {
                post.ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id);
                post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == user.Id && x.IsLiked == true);
            }

            return posts;
        }

        public async Task<Tag> ExtractTagById(string id)
        {
            return await this.db.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}