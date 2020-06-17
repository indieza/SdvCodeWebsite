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

    public class ApprovePostTests
    {
        [Fact]
        public async Task ApprovePost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var targetUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test1",
                PostStatus = PostStatus.Pending,
                ApplicationUserId = targetUser.Id,
                ShortContent = "short content",
            };

            var pendingPost = new PendingPost
            {
                PostId = post.Id,
                IsPending = true,
                ApplicationUserId = post.ApplicationUserId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.AddRange(post);
                db.Users.AddRange(user, targetUser);
                db.PendingPosts.Add(pendingPost);
                await db.SaveChangesAsync();
                var result = await commentService.ApprovePost(post.Id, user);

                Assert.True(result);
                Assert.False(db.PendingPosts.FirstOrDefault(x => x.PostId == post.Id).IsPending);
            }
        }

        [Fact]
        public async Task ApproveNoneExistingPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                var result = await commentService.ApprovePost(Guid.NewGuid().ToString(), user);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task PostIsApproved()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };

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

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.AddRange(post);
                db.PendingPosts.Add(pendingPost);
                await db.SaveChangesAsync();
                var result = await commentService.ApprovePost(post.Id, user);

                Assert.False(result);
                Assert.False(db.PendingPosts.FirstOrDefault(x => x.PostId == post.Id).IsPending);
            }
        }
    }
}