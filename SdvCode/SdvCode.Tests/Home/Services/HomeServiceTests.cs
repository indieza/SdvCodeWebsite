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

    public class HomeServiceTests
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

            using (var db = new ApplicationDbContext(options))
            {
                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = await homeService.CreateRole("Administrator");
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public async Task TestGetAllAdministratorsZeroResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();

            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator" });

            using (var db = new ApplicationDbContext(options))
            {
                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = await homeService.GetAllAdministrators();

                Assert.Equal(0, result.Count);
            }
        }

        [Fact]
        public async Task TestGetAllAdministrators()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();

            var role = new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrator",
                RoleLevel = 1,
            };
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "pesho",
                Email = "pesho@gmail.com",
            };

            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationRole { Id = role.Id, Name = "Administrator" });

            using (var db = new ApplicationDbContext(options))
            {
                db.Users.Add(user);
                db.Roles.Add(role);
                db.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                });

                db.SaveChanges();

                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = await homeService.GetAllAdministrators();

                Assert.Equal(1, result.Count);
            }
        }

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

                IHomeService homeService = new HomeService(db, roleManagerMock.Object);
                var result = homeService.GetRegisteredUsersCount();

                Assert.Equal(1, result);
            }
        }
    }
}