namespace SdvCode.Tests.Blog.Services
{
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ExtractAllCategoryNamesTests
    {
        [Fact]
        public async Task ExtractAllCategoryNames()
        {
            var category1 = new Category { Id = Guid.NewGuid().ToString(), Name = "Test1" };
            var category2 = new Category { Id = Guid.NewGuid().ToString(), Name = "Test2" };

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
                db.Categories.AddRange(category1, category2);
                await db.SaveChangesAsync();
                var result = await blogService.ExtractAllCategoryNames();

                Assert.Equal(2, result.Count);
                Assert.Equal("Test1", result.First());
            }
        }
    }
}