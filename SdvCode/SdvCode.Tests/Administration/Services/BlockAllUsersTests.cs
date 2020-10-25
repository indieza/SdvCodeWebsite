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

    public class BlockAllUsersTests
    {
        [Fact]
        public async Task BlockAllUsers()
        {
            var user1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test1", IsBlocked = false };
            var user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test2", IsBlocked = false };
            var user3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test3", IsBlocked = false };
            var adminRole =
                new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", RoleLevel = 1 };
            var editorRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Editor", RoleLevel = 2 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(adminRole);

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
            //    db.Users.AddRange(user1, user2, user3);
            //    db.Roles.AddRange(adminRole);
            //    db.UserRoles.AddRange(new IdentityUserRole<string>
            //    {
            //        RoleId = adminRole.Id,
            //        UserId = user2.Id
            //    },
            //    new IdentityUserRole<string>
            //    {
            //        RoleId = editorRole.Id,
            //        UserId = user1.Id,
            //    },
            //    new IdentityUserRole<string>
            //    {
            //        RoleId = editorRole.Id,
            //        UserId = user3.Id,
            //    });

            //    await db.SaveChangesAsync();
            //    var result = await userPenaltiesSecvice.BlockAllUsers();

            //    Assert.Equal(2, result);
            //    Assert.True(db.Users.FirstOrDefault(x => x.UserName == user1.UserName).IsBlocked);
            //}
        }
    }
}