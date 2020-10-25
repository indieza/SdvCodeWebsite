namespace SdvCode.Tests.Profile.Services
{
    using Microsoft.AspNetCore.Identity;
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

    public class ExtractUserInfoTests
    {
        [Fact]
        public async Task TestExtractUserInfo()
        {
            var currentUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "gogo" };
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Editor" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.Users.AddRange(user, currentUser);
            //    db.Roles.Add(role);
            //    db.UserRoles.Add(new IdentityUserRole<string>
            //    {
            //        RoleId = role.Id,
            //        UserId = user.Id,
            //    });
            //    db.FollowUnfollows.Add(new FollowUnfollow
            //    {
            //        PersonId = user.Id,
            //        FollowerId = currentUser.Id,
            //        IsFollowed = true
            //    });

            //    await db.SaveChangesAsync();
            //    var result = await profileService.ExtractUserInfo(user.UserName, currentUser);

            //    Assert.True(result.IsFollowed);
            //    Assert.False(result.IsBlocked);
            //    Assert.Single(result.Roles);
            //}
        }
    }
}