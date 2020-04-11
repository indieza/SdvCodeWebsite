namespace SdvCode.Tests.Comment.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Comment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class IsCommentIdCorrectTests
    {
        [Fact]
        public async Task IsCommentIdCorrectNullComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            using (var db = new ApplicationDbContext(options))
            {
                ICommentService commentService = new CommentService(db, mockUserManager.Object);
                var result =
                    await commentService.IsCommentIdCorrect(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsCommentIdCorrectNullPost()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            using (var db = new ApplicationDbContext(options))
            {
                ICommentService commentService = new CommentService(db, mockUserManager.Object);
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                var result =
                    await commentService.IsCommentIdCorrect(comment.Id, Guid.NewGuid().ToString());

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsCommentIdCorrectIncorrectPostId()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
                PostId = Guid.NewGuid().ToString(),
            };
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            using (var db = new ApplicationDbContext(options))
            {
                ICommentService commentService = new CommentService(db, mockUserManager.Object);
                db.Comments.Add(comment);
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                var result =
                    await commentService.IsCommentIdCorrect(comment.Id, post.Id);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsCommentIdCorrect()
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
            };
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
                PostId = post.Id,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            using (var db = new ApplicationDbContext(options))
            {
                ICommentService commentService = new CommentService(db, mockUserManager.Object);
                db.Comments.Add(comment);
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                var result =
                    await commentService.IsCommentIdCorrect(comment.Id, post.Id);

                Assert.True(result);
            }
        }
    }
}