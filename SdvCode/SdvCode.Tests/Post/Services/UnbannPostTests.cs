namespace SdvCode.Tests.Post.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Editor.Services.Post;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class UnbannPostTests
    {
        [Fact]
        public async Task UnbannNoneExistingPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                var result = await commentService.UnbannPost(Guid.NewGuid().ToString());

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnbannUnbannedPost()
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Title = "Test",
                PostStatus = PostStatus.Approved,
            };

            var blockPost = new BlockedPost
            {
                PostId = post.Id,
                ApplicationUserId = post.ApplicationUserId,
                IsBlocked = false,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockPost);
                await db.SaveChangesAsync();
                var result = await commentService.UnbannPost(post.Id);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnbannPost()
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Title = "Test",
                PostStatus = PostStatus.Approved,
            };

            var blockPost = new BlockedPost
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
                db.BlockedPosts.Add(blockPost);
                await db.SaveChangesAsync();
                var result = await commentService.UnbannPost(post.Id);

                Assert.True(result);
                Assert.False(db.BlockedPosts.FirstOrDefault(x => x.PostId == post.Id).IsBlocked);
            }
        }
    }
}