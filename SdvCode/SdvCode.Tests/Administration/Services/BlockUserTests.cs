namespace SdvCode.Tests.Administration.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.ML.Trainers.FastTree;
    using Moq;
    using SdvCode.Areas.Administration.Services.UserPenalties;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class BlockUserTests
    {
        [Fact]
        public async Task BlockAdministrator()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test" };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", RoleLevel = 1 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
            //    db.Users.AddRange(user);
            //    db.Roles.AddRange(role);
            //    db.UserRoles.AddRange(new IdentityUserRole<string>
            //    {
            //        RoleId = role.Id,
            //        UserId = user.Id,
            //    });
            //    await db.SaveChangesAsync();
            //    //var result = await userPenaltiesSecvice.BlockUser(user.UserName);

            //    //Assert.False(result);
            //    //Assert.False(db.Users.FirstOrDefault(x => x.Id == user.Id).IsBlocked);
            //}
        }

        [Fact]
        public async Task BlockBlockedUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test", IsBlocked = true };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", RoleLevel = 1 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
            //    db.Users.AddRange(user);
            //    db.Roles.AddRange(role);
            //    db.UserRoles.AddRange(new IdentityUserRole<string>
            //    {
            //        RoleId = role.Id,
            //        UserId = user.Id,
            //    });
            //    await db.SaveChangesAsync();
            //    var result = await userPenaltiesSecvice.BlockUser(user.UserName);

            //    Assert.False(result);
            //    Assert.True(db.Users.FirstOrDefault(x => x.Id == user.Id).IsBlocked);
            //}
        }

        [Fact]
        public async Task BlockUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test", IsBlocked = false };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", RoleLevel = 1 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
            //    db.Users.AddRange(user);
            //    db.Roles.AddRange(role);
            //    await db.SaveChangesAsync();
            //    var result = await userPenaltiesSecvice.BlockUser(user.UserName);

            //    Assert.True(result);
            //    Assert.True(db.Users.FirstOrDefault(x => x.Id == user.Id).IsBlocked);
            //}
        }
    }
}