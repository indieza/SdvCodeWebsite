// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserPosts
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
    using X.PagedList;

    public class UserPostsService : GlobalPostsExtractor, IUserPostsService
    {
        private readonly ApplicationDbContext db;

        public UserPostsService(ApplicationDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<ICollection<PostViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser currentUser)
        {
            var posts = await this.db.Posts.Where(x => x.ApplicationUser.UserName == username).ToListAsync();
            List<PostViewModel> postsModel = await this.ExtractPosts(currentUser, posts);

            return postsModel;
        }

        public async Task<ICollection<PostViewModel>> ExtractLikedPostsByUsername(string username, ApplicationUser currentUser)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var postsIds = await this.db.PostsLikes.Where(x => x.UserId == user.Id && x.IsLiked == true).Select(x => x.PostId).ToListAsync();

            List<Post> posts = new List<Post>();

            foreach (var postId in postsIds)
            {
                posts.Add(await this.db.Posts.FirstOrDefaultAsync(x => x.Id == postId));
            }

            List<PostViewModel> postsModel = await this.ExtractPosts(currentUser, posts);

            return postsModel;
        }
    }
}