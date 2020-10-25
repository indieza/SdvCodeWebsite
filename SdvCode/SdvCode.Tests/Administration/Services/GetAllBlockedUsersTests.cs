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
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class GetAllBlockedUsersTests
    {
        [Fact]
        public async Task UnblockUser()
        {
            var user1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test1", IsBlocked = true };
            var user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test2", IsBlocked = true };
            var user3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test3", IsBlocked = true };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
            //    db.Users.AddRange(user1, user2, user3);
            //    await db.SaveChangesAsync();
            //    var result = userPenaltiesSecvice.GetAllBlockedUsers();

            //    Assert.Equal(3, result.Count);
            //    Assert.Contains(user3.UserName, result);
            //}
        }
    }
}