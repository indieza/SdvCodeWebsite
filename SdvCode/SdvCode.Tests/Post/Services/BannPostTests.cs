namespace SdvCode.Tests.Post.Services
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.Services.Post;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class BannPostTests
    {
        [Fact]
        public async Task BannNoneExistingPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                var result = await commentService.BannPost(Guid.NewGuid().ToString());

                Assert.False(result);
            }
        }

        [Fact]
        public async Task BannBannedPost()
        {
            Post post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test",
                ApplicationUserId = Guid.NewGuid().ToString(),
            };

            BlockedPost blockedPost = new BlockedPost
            {
                PostId = post.Id,
                ApplicationUserId = post.ApplicationUserId,
                IsBlocked = true,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockedPost);
                await db.SaveChangesAsync();
                var result = await commentService.BannPost(post.Id);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task BannPost()
        {
            Post post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test",
                ApplicationUserId = Guid.NewGuid().ToString(),
            };

            BlockedPost blockedPost = new BlockedPost
            {
                PostId = post.Id,
                ApplicationUserId = post.ApplicationUserId,
                IsBlocked = false,
            };

            FavouritePost favouritePost = new FavouritePost
            {
                ApplicationUserId = post.ApplicationUserId,
                IsFavourite = true,
                PostId = post.Id,
            };

            UserAction userAction = new UserAction
            {
                Action = UserActionsType.CreatePost,
                PostId = post.Id,
            };

            PostLike postLike = new PostLike
            {
                PostId = post.Id,
                UserId = post.ApplicationUserId,
                IsLiked = true,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockedPost);
                db.FavouritePosts.Add(favouritePost);
                db.UserActions.Add(userAction);
                db.PostsLikes.Add(postLike);
                await db.SaveChangesAsync();
                var result = await commentService.BannPost(post.Id);

                Assert.True(result);
                Assert.Equal(1, db.BlockedPosts.Count());
                Assert.Equal(PostStatus.Banned, db.Posts.FirstOrDefault(x => x.Id == post.Id).PostStatus);
                Assert.Equal(0, db.UserActions.Count());
            }
        }
    }
}