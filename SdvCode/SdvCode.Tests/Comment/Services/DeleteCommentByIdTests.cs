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
    using SdvCode.Models.User;
    using SdvCode.Services.Comment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class DeleteCommentByIdTests
    {
        [Fact]
        public async Task DeleteExistingCommentById()
        {
            var comment = new Comment { Id = Guid.NewGuid().ToString(), Content = "Test1" };

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
                db.Comments.AddRange(comment);
                await db.SaveChangesAsync();
                var result = await commentService.DeleteCommentById(comment.Id);

                Assert.Equal(0, db.Comments.Count());
                Assert.Equal("Success", result.Item1);
                Assert.Equal(SuccessMessages.SuccessfullyDeletePostComment, result.Item2);
            }
        }

        [Fact]
        public async Task DeleteNotExistingCommentById()
        {
            var comment = new Comment { Id = Guid.NewGuid().ToString(), Content = "Test1" };

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
                var result = await commentService.DeleteCommentById(comment.Id);

                Assert.Equal(0, db.Comments.Count());
                Assert.Equal("Error", result.Item1);
                Assert.Equal(ErrorMessages.InvalidInputModel, result.Item2);
            }
        }

        [Fact]
        public async Task DeleteExistingCommentByIdWithChildren()
        {
            var comment1 =
                new Comment { Id = Guid.NewGuid().ToString(), Content = "Test1" };
            var comment2 =
                new Comment { Id = Guid.NewGuid().ToString(), Content = "Test2", ParentCommentId = comment1.Id };
            var comment3 =
                new Comment { Id = Guid.NewGuid().ToString(), Content = "Test3", ParentCommentId = comment2.Id };

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
                db.Comments.AddRange(comment1, comment2, comment3);
                await db.SaveChangesAsync();
                var result = await commentService.DeleteCommentById(comment1.Id);

                Assert.Equal(0, db.Comments.Count());
                Assert.Equal("Success", result.Item1);
                Assert.Equal(SuccessMessages.SuccessfullyDeletePostComment, result.Item2);
            }
        }
    }
}