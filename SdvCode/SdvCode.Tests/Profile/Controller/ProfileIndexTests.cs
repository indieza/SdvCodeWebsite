namespace SdvCode.Tests.Profile.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Controllers;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ProfileIndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModel()
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

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleManagerMock =
                new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

            var controller =
                new ProfileController(mockUserManager.Object, roleManagerMock.Object, mockService.Object);
            var result = await controller.Index("indieza", ProfileTab.Activities, 0);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsType<ProfileViewModel>(viewResult.Model);

            var viewModel = viewResult.Model as ProfileViewModel;
            Assert.Equal(ProfileTab.Activities, viewModel.ActiveTab);
            Assert.Equal(0, viewModel.Page);
        }
    }
}