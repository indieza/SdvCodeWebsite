﻿namespace SdvCode.Tests.Post.Services
{
    using Microsoft.AspNetCore.Authentication.Cookies;
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

    public class BannPostTests
    {
        [Fact]
        public async Task BannNoneExistingPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                var result = await commentService.BannPost(Guid.NewGuid().ToString(), user);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task BannBannedPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };

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

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockedPost);
                await db.SaveChangesAsync();
                var result = await commentService.BannPost(post.Id, user);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task BannPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };

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
                //ActionType = UserActionType.CreatePost,
                //PostId = post.Id,
            };

            PostLike postLike = new PostLike
            {
                PostId = post.Id,
                UserId = post.ApplicationUserId,
                IsLiked = true,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                IEditorPostService commentService = new EditorPostService(db, mockService.Object, mockHub.Object);
                db.Posts.Add(post);
                db.BlockedPosts.Add(blockedPost);
                db.FavouritePosts.Add(favouritePost);
                db.UserActions.Add(userAction);
                db.PostsLikes.Add(postLike);
                await db.SaveChangesAsync();
                var result = await commentService.BannPost(post.Id, user);

                Assert.True(result);
                Assert.Equal(1, db.BlockedPosts.Count());
                Assert.Equal(PostStatus.Banned, db.Posts.FirstOrDefault(x => x.Id == post.Id).PostStatus);
                Assert.Equal(0, db.UserActions.Count());
            }
        }
    }
}