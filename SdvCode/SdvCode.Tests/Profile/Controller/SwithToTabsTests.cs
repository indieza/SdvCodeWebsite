namespace SdvCode.Tests.Profile.Controller
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Controllers;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class SwithToTabsTests
    {
        [Fact]
        public async Task ShouldReturnRedirectToAction()
        {
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
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));
            mockUserManager.Setup(x => x.FindByNameAsync("indieza")).ReturnsAsync(new ApplicationUser
            {
                UserName = "indieza",
            });

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            var controller =
                new ProfileController(mockUserManager.Object, roleManagerMock.Object, mockService.Object);
            var result = (RedirectToActionResult)await controller.SwitchToAllActivitiesTabs("indieza", "Activities");

            Assert.Equal("Index", result.ActionName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.True(result.RouteValues.ContainsKey("tab"));
            Assert.True(result.RouteValues.ContainsKey("username"));
            Assert.Equal(ProfileTab.Activities, result.RouteValues["tab"]);
            Assert.Equal("indieza", result.RouteValues["username"]);
        }
    }
}