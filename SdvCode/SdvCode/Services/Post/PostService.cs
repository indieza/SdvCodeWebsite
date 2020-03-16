// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.ViewModels.Post.ViewModels;

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;

        public PostService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public PostViewModel ExtractCurrentPost(string id, string title)
        {
            var post = this.db.Posts.FirstOrDefault(x => x.Id == id && x.Title == title);
            PostViewModel model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Likes = post.Likes,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                Category = post.Category,
                UpdatedOn = post.UpdatedOn,
                Comments = post.Comments,
                ImageUrl = post.ImageUrl,
            };

            model.ApplicationUser = this.db.Users.FirstOrDefault(x => x.Id == post.ApplicationUserId);

            foreach (var tag in post.PostsTags)
            {
                var curretnTag = this.db.Tags.FirstOrDefault(x => x.Id == tag.TagId);
                model.Tags.Add(new Tag
                {
                    Id = curretnTag.Id,
                    CreatedOn = curretnTag.CreatedOn,
                    Name = curretnTag.Name,
                });
            }

            return model;
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