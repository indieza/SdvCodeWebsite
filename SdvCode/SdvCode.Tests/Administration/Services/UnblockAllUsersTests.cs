namespace SdvCode.Tests.Administration.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
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

    public class UnblockAllUsersTests
    {
        [Fact]
        public async Task BlockAllUsers()
        {
            var user1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test1", IsBlocked = true };
            var user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test2", IsBlocked = false };
            var user3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test3", IsBlocked = true };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            using (var db = new ApplicationDbContext(options))
            {
                IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
                db.Users.AddRange(user1, user2, user3);

                await db.SaveChangesAsync();
                var result = await userPenaltiesSecvice.UnblockAllUsers();

                Assert.Equal(2, result);
                Assert.False(db.Users.FirstOrDefault(x => x.UserName == user1.UserName).IsBlocked);
            }
        }
    }
}