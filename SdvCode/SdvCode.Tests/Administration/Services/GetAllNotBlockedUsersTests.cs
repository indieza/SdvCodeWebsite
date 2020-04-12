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

    public class GetAllNotBlockedUsersTests
    {
        [Fact]
        public async Task GetAllNotBlockedUsers()
        {
            var user1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test1", IsBlocked = false };
            var user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test2", IsBlocked = false };
            var user3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "Test3", IsBlocked = false };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", RoleLevel = 1 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);
            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            using (var db = new ApplicationDbContext(options))
            {
                IUsersPenaltiesService userPenaltiesSecvice = new UsersPenaltiesService(db, roleManagerMock.Object);
                db.Users.AddRange(user1, user2, user3);
                db.Roles.AddRange(role);
                db.UserRoles.AddRange(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user2.Id
                });

                await db.SaveChangesAsync();
                var result = await userPenaltiesSecvice.GetAllNotBlockedUsers();

                Assert.Equal(2, result.Count);
                Assert.DoesNotContain(user2.UserName, result);
            }
        }
    }
}