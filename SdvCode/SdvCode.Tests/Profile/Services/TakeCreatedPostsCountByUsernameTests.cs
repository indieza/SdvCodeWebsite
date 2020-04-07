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

    public class TakeCreatedPostsCountByUsernameTests
    {
        [Fact]
        public async Task TestTakeCreatedPostsCountByUsername()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var post = new Post { Id = Guid.NewGuid().ToString(), ApplicationUser = user };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new ApplicationDbContext(options))
            {
                IProfileService profileService = new ProfileService(db);
                db.Users.Add(user);
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                var result = await profileService.TakeCreatedPostsCountByUsername(user.UserName);

                Assert.Equal(1, result);
            }
        }
    }
}