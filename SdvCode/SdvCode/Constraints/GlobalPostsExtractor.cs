// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Constraints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Post.ViewModels;

    public class GlobalPostsExtractor
    {
        private readonly ApplicationDbContext db;

        public GlobalPostsExtractor(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<List<PostViewModel>> ExtractPosts(ApplicationUser user, List<Post> posts)
        {
            List<PostViewModel> postsModel = new List<PostViewModel>();

            foreach (var post in posts)
            {
                PostViewModel currentPostModel = new PostViewModel
                {
                    Id = post.Id,
                    ImageUrl = post.ImageUrl,
                    Content = post.Content,
                    ShortContent = post.ShortContent,
                    ApplicationUserId = post.ApplicationUserId,
                    CategoryId = post.CategoryId,
                    CreatedOn = post.CreatedOn,
                    Title = post.Title,
                    UpdatedOn = post.UpdatedOn,
                    ApplicationUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == post.ApplicationUserId),
                    Category = await this.db.Categories.FirstOrDefaultAsync(x => x.Id == post.CategoryId),
                    Likes = post.Likes,
                    PostStatus = post.PostStatus,
                    Comments = this.db.Comments
                    .Where(x => x.PostId == post.Id && x.CommentStatus == CommentStatus.Approved)
                    .ToList(),
                };

                if (user != null)
                {
                    currentPostModel.IsLiked = this.db.PostsLikes.Any(x => x.PostId == post.Id && x.UserId == user.Id && x.IsLiked == true);
                    currentPostModel.IsFavourite = this.db.FavouritePosts
                        .Any(x => x.ApplicationUserId == user.Id && x.PostId == post.Id && x.IsFavourite == true);
                    currentPostModel.IsAuthor = user.Id == post.ApplicationUserId ? true : false;
                }

                var usersIds = this.db.PostsLikes.Where(x => x.PostId == post.Id && x.IsLiked == true).Select(x => x.UserId).ToList();
                foreach (var userId in usersIds)
                {
                    currentPostModel.Likers.Add(this.db.Users.FirstOrDefault(x => x.Id == userId));
                }

                foreach (var tag in post.PostsTags)
                {
                    var curretnTag = this.db.Tags.FirstOrDefault(x => x.Id == tag.TagId);
                    currentPostModel.Tags.Add(new Tag
                    {
                        Id = curretnTag.Id,
                        CreatedOn = curretnTag.CreatedOn,
                        Name = curretnTag.Name,
                    });
                }

                postsModel.Add(currentPostModel);
            }

            return postsModel;
        }
    }
}