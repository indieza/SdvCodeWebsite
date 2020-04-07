namespace SdvCode.Tests.Profile.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class FollowUserTests
    {
        [Fact]
        public async Task TestFollowUser()
        {
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IProfileService profileService = new ProfileService(db);
                db.Users.AddRange(user, currentUser);
                await db.SaveChangesAsync();
                var result = await profileService.FollowUser(user.UserName, currentUser);

                Assert.Equal(currentUser, result);
                Assert.Equal(2, db.UserActions.Count());
            }
        }

        [Fact]
        public async Task TestFollowUserRepeatedAction()
        {
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IProfileService profileService = new ProfileService(db);
                db.Users.AddRange(user, currentUser);
                await db.SaveChangesAsync();
                await profileService.FollowUser(user.UserName, currentUser);
                var result = await profileService.FollowUser(user.UserName, currentUser);

                Assert.Equal(currentUser, result);
                Assert.Equal(2, db.UserActions.Count());
            }
        }
    }
}