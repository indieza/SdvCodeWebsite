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
    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;
    using X.PagedList;

    public class UserPostsService : GlobalPostsExtractor, IUserPostsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPostsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
            : base(db)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<ICollection<PostViewModel>> ExtractCreatedPostsByUsername(string username, ApplicationUser currentUser)
        {
            var posts = this.db.Posts.Where(x => x.ApplicationUser.UserName == username).ToList();
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = posts
                    .Where(x => x.PostStatus == PostStatus.Banned || x.PostStatus == PostStatus.Pending || x.PostStatus == PostStatus.Approved)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved &&
                        x.ApplicationUserId == user.Id)
                        .ToList();
                }
                else
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .ToList();
                }
            }

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

            if (currentUser != null &&
                (await this.userManager.IsInRoleAsync(currentUser, Roles.Administrator.ToString()) ||
                await this.userManager.IsInRoleAsync(currentUser, Roles.Editor.ToString())))
            {
                posts = posts
                    .Where(x => x.PostStatus == PostStatus.Banned || x.PostStatus == PostStatus.Pending || x.PostStatus == PostStatus.Approved)
                    .ToList();
            }
            else
            {
                if (currentUser != null)
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved &&
                        x.ApplicationUserId == user.Id)
                        .ToList();
                }
                else
                {
                    posts = posts
                        .Where(x => x.PostStatus == PostStatus.Approved)
                        .ToList();
                }
            }

            List<PostViewModel> postsModel = await this.ExtractPosts(currentUser, posts);

            return postsModel;
        }
    }
}