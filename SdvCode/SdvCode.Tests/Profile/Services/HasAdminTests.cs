namespace SdvCode.Tests.Profile.Services
{
    using Microsoft.AspNetCore.Identity;
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

    public class HasAdminTests
    {
        [Fact]
        public async Task TestHasNoAdminInIfStatment()
        {
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.Roles.Add(role);
            //    await db.SaveChangesAsync();
            //    var result = await profileService.HasAdmin(role);

            //    Assert.False(result);
            //}
        }

        [Fact]
        public async Task TestHasNoAdminInElseStatment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    var result = await profileService.HasAdmin(null);

            //    Assert.False(result);
            //}
        }

        [Fact]
        public async Task TestHasAdminInIfStatment()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.UserRoles.Add(new IdentityUserRole<string>
            //    {
            //        RoleId = role.Id,
            //        UserId = user.Id,
            //    });
            //    await db.SaveChangesAsync();
            //    var result = await profileService.HasAdmin(role);

            //    Assert.True(result);
            //}
        }
    }
}