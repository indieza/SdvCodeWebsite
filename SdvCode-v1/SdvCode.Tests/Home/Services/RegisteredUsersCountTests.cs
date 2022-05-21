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
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class RegisteredUsersCountTests
    {
        [Fact]
        public void GetRegisteredUsersCountTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                db.Users.Add(new ApplicationUser
                {
                    UserName = "pesho",
                });

                db.SaveChanges();

                //IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                //var result = homeService.GetRegisteredUsersCount();

                //Assert.Equal(1, result);
            }
        }
    }
}