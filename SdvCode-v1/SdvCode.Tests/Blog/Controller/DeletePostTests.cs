namespace SdvCode.Tests.Blog.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Areas.Administration.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Controllers;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.InputModels;

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Twilio.Rest.Autopilot.V1.Assistant;

    using Xunit;

    public class DeletePostTests
    {
        //[Fact]
        //public async Task ViewShouldRedirectCorrectForBlocekdUser()
        //{
        //    var currentUser = new ApplicationUser { UserName = "gogo", IsBlocked = true };

        //    var mockService = new Mock<IBlogService>();
        //    mockService.Setup(x => x.IsBlocked(currentUser))
        //        .Returns(true);

        //    var mockUserManager = new Mock<UserManager<ApplicationUser>>(
        //            new Mock<IUserStore<ApplicationUser>>().Object,
        //            new Mock<IOptions<IdentityOptions>>().Object,
        //            new Mock<IPasswordHasher<ApplicationUser>>().Object,
        //            new IUserValidator<ApplicationUser>[0],
        //            new IPasswordValidator<ApplicationUser>[0],
        //            new Mock<ILookupNormalizer>().Object,
        //            new Mock<IdentityErrorDescriber>().Object,
        //            new Mock<IServiceProvider>().Object,
        //            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        //    mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        //         .ReturnsAsync(currentUser);

        //    var httpContext = new DefaultHttpContext();
        //    var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        //    var controller = new BlogController(mockService.Object, mockUserManager.Object)
        //    {
        //        TempData = tempData,
        //    };

        //    var result = await controller.DeletePost(Guid.NewGuid().ToString());
        //    Assert.IsType<RedirectToActionResult>(result);

        //    var redirect = result as RedirectToActionResult;
        //    Assert.Equal($"Index", redirect.ActionName);
        //    Assert.Equal($"Blog", redirect.ControllerName);

        //    Assert.True(controller.TempData.ContainsKey("Error"));
        //    Assert.Equal(ErrorMessages.YouAreBlock, controller.TempData["Error"]);
        //}

        //[Fact]
        //public async Task ViewShouldRedirectCorrectForNoInRoleUser()
        //{
        //    var currentUser = new ApplicationUser { UserName = "gogo" };

        //    var mockService = new Mock<IBlogService>();
        //    mockService.Setup(x => x.IsInPostRole(currentUser, Guid.NewGuid().ToString()))
        //        .ReturnsAsync(false);

        //    var mockUserManager = new Mock<UserManager<ApplicationUser>>(
        //            new Mock<IUserStore<ApplicationUser>>().Object,
        //            new Mock<IOptions<IdentityOptions>>().Object,
        //            new Mock<IPasswordHasher<ApplicationUser>>().Object,
        //            new IUserValidator<ApplicationUser>[0],
        //            new IPasswordValidator<ApplicationUser>[0],
        //            new Mock<ILookupNormalizer>().Object,
        //            new Mock<IdentityErrorDescriber>().Object,
        //            new Mock<IServiceProvider>().Object,
        //            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        //    mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        //         .ReturnsAsync(currentUser);

        //    var httpContext = new DefaultHttpContext();
        //    var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        //    var controller = new BlogController(mockService.Object, mockUserManager.Object)
        //    {
        //        TempData = tempData,
        //    };

        //    var result = await controller.DeletePost(Guid.NewGuid().ToString());
        //    Assert.IsType<RedirectToActionResult>(result);

        //    var redirect = result as RedirectToActionResult;
        //    Assert.Equal($"Index", redirect.ActionName);
        //    Assert.Equal($"Blog", redirect.ControllerName);

        //    Assert.True(controller.TempData.ContainsKey("Error"));
        //    Assert.Equal(ErrorMessages.NotInDeletePostRoles, controller.TempData["Error"]);
        //}
    }
}