namespace SdvCode.Tests.Blog.Controller
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Controllers;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Blog;
    using SdvCode.ViewModels.Blog.ViewModels;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class IndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModel()
        {
            var currentUser = new ApplicationUser { UserName = "gogo" };

            var mockService = new Mock<IBlogService>();
            //mockService.Setup(x => x.ExtraxtAllPosts(currentUser, null))
            //    .ReturnsAsync(new List<PostViewModel>
            //    {
            //        new PostViewModel
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            Title = "Test",
            //        }
            //    });

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

            //var controller =
            //    new BlogController(mockService.Object, mockUserManager.Object);
            //var result = await controller.Index(1, null);
            //Assert.IsType<ViewResult>(result);

            //var viewResult = result as ViewResult;
            //Assert.IsType<BlogViewModel>(viewResult.Model);

            //var viewModel = viewResult.Model as BlogViewModel;
            //Assert.Single(viewModel.Posts);
            //Assert.Equal("Test", viewModel.Posts.ToList()[0].Title);
        }
    }
}