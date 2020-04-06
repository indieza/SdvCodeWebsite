namespace SdvCode.Tests.Home.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.Services.Home;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class HomeServiceTests
    {
        [Fact]
        public async Task TestCreateRole()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            roleStore.Setup(x =>
                x.CreateAsync(new ApplicationRole { Name = "Administrator" }, CancellationToken.None))
                .Returns(Task.FromResult(IdentityResult.Success));

            var roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                         roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = await homeService.CreateRole("Administrator");
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public void TestGetAllAdministrators()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            roleStore.Setup(x =>
                x.FindByNameAsync("Administrator", CancellationToken.None))
                .Returns(Task.FromResult(new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                }));

            var roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                         roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = homeService.GetAllAdministrators();
            }
        }
    }
}