namespace SdvCode.Tests.Blog.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Post.InputModels;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractPostTests
    {
        [Fact]
        public async Task ExtractPost()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var category = new Category { Id = Guid.NewGuid().ToString(), Name = "Test" };
            var post = new Post { Id = Guid.NewGuid().ToString(), Title = "Test1", CategoryId = category.Id };

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
                IBlogService blogService = new BlogService(db, null, mockUserManager.Object);
                db.Categories.Add(category);
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                var result = await blogService.ExtractPost(post.Id, user);

                Assert.IsType<EditPostInputModel>(result);

                var model = result as EditPostInputModel;
                Assert.Equal(post.Title, model.Title);
            }
        }
    }
}