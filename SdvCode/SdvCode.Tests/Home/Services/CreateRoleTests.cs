namespace SdvCode.Tests.Home.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.Services.Home;

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class CreateRoleTests
    {
        [Fact]
        public async Task TestCreateRole()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();

            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IHomeService homeService = new HomeService(db, roleManagerMock.Object);
            //    var result = await homeService.CreateRole("Administrator");
            //    Assert.True(result.Succeeded);
            //}
        }
    }
}