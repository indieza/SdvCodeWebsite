namespace SdvCode.Tests.Tag.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Tag;

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class ExtractTagByIdTests
    {
        [Fact]
        public async Task TestExtractTagById()
        {
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
            var tag = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test Tag" };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                //ITagService tagService = new TagService(db, mockUserManager.Object);
                //db.Tags.Add(tag);
                //await db.SaveChangesAsync();
                //var result = await tagService.ExtractTagById(tag.Id);

                //Assert.Equal(tag, result);
                //Assert.Equal(tag.Id, result.Id);
            }
        }
    }
}