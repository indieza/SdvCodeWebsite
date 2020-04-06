namespace SdvCode.Tests.Profile.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SdvCode.Constraints;
    using SdvCode.Controllers;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
    using SdvCode.ViewModels.Users.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class AllUsersTests
    {
        [Fact]
        public async Task AllUsersShouldReturnCorrectViewModel()
        {
            var currentUser = new ApplicationUser { UserName = "gogo" };
            var user = new ApplicationUser { UserName = "pesho" };

            var mockService = new Mock<IProfileService>();
            mockService.Setup(x => x.GetAllUsers(currentUser, null))
                .ReturnsAsync(new List<UserCardViewModel>()
                {
                    new UserCardViewModel
                    {
                        FirstName = "Pesho",
                        LastName = "Peshov",
                    },
                    new UserCardViewModel
                    {
                        FirstName = "Gogo",
                        LastName = "Gogov",
                    }
                });

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
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(currentUser);

            var controller = new ProfileController(mockUserManager.Object, mockService.Object);

            var result = await controller.AllUsers(null, null);
            Assert.IsType<ViewResult>(result);

            var view = result as ViewResult;
            Assert.IsType<AllUsersViewModel>(view.Model);

            var model = view.Model as AllUsersViewModel;
            Assert.Equal(2, model.UsersCards.Count());
            Assert.Null(model.Search);
        }
    }
}