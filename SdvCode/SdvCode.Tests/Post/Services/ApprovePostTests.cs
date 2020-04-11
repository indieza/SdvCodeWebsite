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

    public class ApprovePostTests
    {
        [Fact]
        public async Task ApprovePost()
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test1",
                PostStatus = PostStatus.Pending,
                ApplicationUserId = Guid.NewGuid().ToString(),
            };

            var pendingPost = new PendingPost
            {
                PostId = post.Id,
                IsPending = true,
                ApplicationUserId = post.ApplicationUserId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                db.Posts.AddRange(post);
                db.PendingPosts.Add(pendingPost);
                db.SaveChanges();
                var result = await commentService.ApprovePost(post.Id);

                Assert.True(result);
                Assert.False(db.PendingPosts.FirstOrDefault(x => x.PostId == post.Id).IsPending);
            }
        }

        [Fact]
        public async Task ApproveNoneExistingPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                var result = await commentService.ApprovePost(Guid.NewGuid().ToString());

                Assert.False(result);
            }
        }

        [Fact]
        public async Task PostIsApproved()
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test1",
                PostStatus = PostStatus.Approved,
                ApplicationUserId = Guid.NewGuid().ToString(),
            };

            var pendingPost = new PendingPost
            {
                PostId = post.Id,
                IsPending = false,
                ApplicationUserId = post.ApplicationUserId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db);
                db.Posts.AddRange(post);
                db.PendingPosts.Add(pendingPost);
                db.SaveChanges();
                var result = await commentService.ApprovePost(post.Id);

                Assert.False(result);
                Assert.False(db.PendingPosts.FirstOrDefault(x => x.PostId == post.Id).IsPending);
            }
        }
    }
}