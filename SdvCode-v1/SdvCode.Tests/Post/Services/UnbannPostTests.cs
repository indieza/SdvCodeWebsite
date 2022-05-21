namespace SdvCode.Tests.Post.Services
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SdvCode.Areas.Editor.Services.Post;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
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
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                var result = await commentService.UnbannPost(Guid.NewGuid().ToString(), user);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnbannUnbannedPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
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

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockPost);
                await db.SaveChangesAsync();
                var result = await commentService.UnbannPost(post.Id, user);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnbannPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
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

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockPost);
                await db.SaveChangesAsync();
                var result = await commentService.UnbannPost(post.Id, user);

                Assert.True(result);
                Assert.False(db.BlockedPosts.FirstOrDefault(x => x.PostId == post.Id).IsBlocked);
            }
        }
    }
}