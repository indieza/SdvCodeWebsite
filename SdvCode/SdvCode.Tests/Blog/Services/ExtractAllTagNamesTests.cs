namespace SdvCode.Tests.Blog.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class ExtractAllTagNamesTests
    {
        [Fact]
        public async Task ExtractAllTagNames()
        {
            var tag1 = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test1" };
            var tag2 = new Tag { Id = Guid.NewGuid().ToString(), Name = "Test2" };

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

            var mockService = new Mock<INotificationService>();

            var mockHub = new Mock<IHubContext<NotificationHub>>();

            using (var db = new ApplicationDbContext(options))
            {
                //IBlogService blogService = new BlogService(db, null, mockUserManager.Object, mockService.Object, mockHub.Object);
                //db.Tags.AddRange(tag1, tag2);
                //await db.SaveChangesAsync();
                //var result = await blogService.ExtractAllTagNames();

                //Assert.Equal(2, result.Count);
                //Assert.Equal("Test1", result.First());
            }
        }
    }
}