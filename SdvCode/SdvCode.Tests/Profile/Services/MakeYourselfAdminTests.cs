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

    public class MakeYourselfAdminTests
    {
        [Fact]
        public async Task TestMakeYourselfAdminNullStatment()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.Users.Add(user);
            //    await db.SaveChangesAsync();
            //    profileService.MakeYourselfAdmin(user.UserName);

            //    Assert.Equal(0, db.UserRoles.Count());
            //}
        }

        [Fact]
        public async Task TestMakeYourselfAdmin()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "peshp" };
            var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Administrator" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.Users.Add(user);
            //    db.Roles.Add(role);
            //    await db.SaveChangesAsync();
            //    profileService.MakeYourselfAdmin(user.UserName);

            //    Assert.Equal(1, db.UserRoles.Count());
            //}
        }
    }
}