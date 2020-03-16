// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Blog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;

        public PostService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> LikePost(string id, string title)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id && x.Title == title);

            if (post != null)
            {
                post.Likes++;
                this.db.Posts.Update(post);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}