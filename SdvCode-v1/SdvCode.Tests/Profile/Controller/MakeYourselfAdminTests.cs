namespace SdvCode.Tests.Profile.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Controllers;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class MakeYourselfAdminTests
    {
        [Fact]
        public void MakeYourselfAdminShouldReturnCorrectViewModel()
        {
            ApplicationUser user = new ApplicationUser { UserName = "pesho" };

            var mockService = new Mock<IProfileService>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            var controller =
                new ProfileController(mockUserManager.Object, roleManagerMock.Object, mockService.Object);

            var result = controller.MakeYourselfAdmin(user.UserName);
            Assert.IsType<RedirectResult>(result);

            //var redirect = result as RedirectResult;
            //Assert.Equal($"/Profile/{user.UserName}", redirect.Url);
        }
    }
}