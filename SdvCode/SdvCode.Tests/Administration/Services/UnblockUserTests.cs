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

    public class UnblockUserTests
    {
        [Fact]
        public async Task UnblockUnblockedUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test", IsBlocked = false };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
                db.Users.AddRange(user);
                await db.SaveChangesAsync();
                var result = await userPenaltiesSecvice.UnblockUser(user.UserName);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnblockNoneExistingUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test", IsBlocked = false };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
                db.Users.AddRange(user);
                await db.SaveChangesAsync();
                var result = await userPenaltiesSecvice.UnblockUser("NoneExisitng");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task UnblockUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test", IsBlocked = true };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
                db.Users.AddRange(user);
                await db.SaveChangesAsync();
                var result = await userPenaltiesSecvice.UnblockUser(user.UserName);

                Assert.True(result);
                Assert.False(db.Users.FirstOrDefault(x => x.Id == user.Id).IsBlocked);
            }
        }
    }
}