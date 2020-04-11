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

    public class ExtractCurrentCommentTests
    {
        [Fact]
        public async Task ExtractCurrentComment()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Test",
                PostId = Guid.NewGuid().ToString(),
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
                    await commentService.ExtractCurrentComment(comment.Id);

                Assert.Equal(1, db.Comments.Count());
                Assert.IsType<EditCommentViewModel>(result);

                var model = result as EditCommentViewModel;
                Assert.Equal("Test", model.Content);
            }
        }
    }
}