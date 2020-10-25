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
    using SdvCode.ViewModels.Comment.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class EditCommentTests
    {
        [Fact]
        public async Task EditCommentNullComment()
        {
            EditCommentViewModel model = new EditCommentViewModel
            {
                CommentId = Guid.NewGuid().ToString(),
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

            //using (var db = new ApplicationDbContext(options))
            //{
            //    ICommentService commentService = new CommentService(db, mockUserManager.Object);
            //    var result =
            //        await commentService.EditComment(model);

            //    Assert.Equal(0, db.Comments.Count());
            //    Assert.Equal("Error", result.Item1);
            //    Assert.Equal(ErrorMessages.InvalidInputModel, result.Item2);
            //}
        }

        [Fact]
        public async Task EditComment()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test1",
                CreatedOn = DateTime.UtcNow,
            };
            EditCommentViewModel model = new EditCommentViewModel
            {
                CommentId = comment.Id,
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

            //using (var db = new ApplicationDbContext(options))
            //{
            //    ICommentService commentService = new CommentService(db, mockUserManager.Object);
            //    db.Comments.Add(comment);
            //    await db.SaveChangesAsync();
            //    var result =
            //        await commentService.EditComment(model);

            //    Assert.Equal(1, db.Comments.Count());
            //    Assert.Equal("Success", result.Item1);
            //    Assert.Equal(SuccessMessages.SuccessfullyEditedComment, result.Item2);
            //    Assert.Equal("Test", db.Comments.FirstOrDefault(x => x.Id == comment.Id).Content);
            //}
        }
    }
}