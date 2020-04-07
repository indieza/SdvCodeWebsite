namespace SdvCode.Tests.Profile.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class GetAllUsersTests
    {
        [Fact]
        public async Task TestGetAllUsersWithoutSearch()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "tetsUser" };
            var user1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var user3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "miro" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IProfileService profileService = new ProfileService(db);
                db.Users.AddRange(user1, user2, user3);
                db.SaveChanges();
                var result = await profileService.GetAllUsers(user, null);

                Assert.Equal(3, result.Count);
                Assert.False(result[0].HasFollowed);
            }
        }
    }
}