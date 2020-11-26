namespace SdvCode.Tests.Profile.Services
{
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class DeleteActivityByIdTests
    {
        [Fact]
        public async Task TestDeleteActivityById()
        {
            //var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "pesho" };
            //var activity = new UserAction { Id = 1, ApplicationUser = user, Action = UserActionsType.Follow };
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //using (var db = new ApplicationDbContext(options))
            //{
            //    IProfileService profileService = new ProfileService(db);
            //    db.Users.Add(user);
            //    db.UserActions.Add(activity);
            //    await db.SaveChangesAsync();
            //    var result = await profileService.DeleteActivityById(user, activity.Id);

            //    Assert.Equal(UserActionsType.Follow.ToString(), result);
            //    Assert.Equal(0, db.UserActions.Count());
            //}
        }
    }
}