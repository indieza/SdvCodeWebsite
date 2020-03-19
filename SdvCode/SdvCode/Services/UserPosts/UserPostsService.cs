// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;

    public class UserPostsService : IUserPostsService
    {
        private readonly ApplicationDbContext db;

        public UserPostsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<Post>> ExtractCreatedPostsByUsername(string username, ApplicationUser curentUser)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var posts = await this.db.Posts.Where(x => x.ApplicationUser.UserName == username).ToListAsync();

            foreach (var post in posts)
            {
                post.ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id);
                post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == curentUser.Id && x.IsLiked == true);
                post.IsFavourite = this.db.FavouritePosts
                    .Any(x => x.ApplicationUserId == user.Id && x.PostId == post.Id && x.IsFavourite == true);

                var usersIds = this.db.PostsLikes.Where(x => x.PostId == post.Id && x.IsLiked == true).Select(x => x.UserId).ToList();
                foreach (var userId in usersIds)
                {
                    post.Likers.Add(this.db.Users.FirstOrDefault(x => x.Id == userId));
                }
            }

            return posts;
        }

        public async Task<ICollection<Post>> ExtractLikedPostsByUsername(string username, ApplicationUser curentUser)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var postsIds = await this.db.PostsLikes.Where(x => x.UserId == user.Id && x.IsLiked == true).Select(x => x.PostId).ToListAsync();

            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(await this.db.Posts.FirstOrDefaultAsync(x => x.Id == postId));
            }

            foreach (var post in posts)
            {
                post.ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId);
                post.Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId);
                post.Likes = this.db.PostsLikes.Count(x => x.PostId == post.Id);
                post.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == curentUser.Id && x.IsLiked == true);
                post.IsFavourite = this.db.FavouritePosts
                    .Any(x => x.ApplicationUserId == user.Id && x.PostId == post.Id && x.IsFavourite == true);

                var usersIds = this.db.PostsLikes.Where(x => x.PostId == post.Id && x.IsLiked == true).Select(x => x.UserId).ToList();
                foreach (var userId in usersIds)
                {
                    post.Likers.Add(this.db.Users.FirstOrDefault(x => x.Id == userId));
                }
            }

            return posts;
        }
    }
}