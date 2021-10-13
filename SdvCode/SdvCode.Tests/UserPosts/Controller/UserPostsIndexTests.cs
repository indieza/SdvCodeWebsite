namespace SdvCode.Tests.UserPosts.Controller
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Controllers;
    using SdvCode.DataViewModels.Blog;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.UserPosts;
    using SdvCode.ViewModels.UserPosts;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class UserPostsIndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModelForLikedPosts()
        {
            var user = new ApplicationUser { UserName = "pesho" };
            var currentUser = new ApplicationUser { UserName = "gogo" };
            var mockService = new Mock<IUserPostsService>();
            mockService
                .Setup(x => x.ExtractLikedPostsByUsername(user.UserName, currentUser))
                .ReturnsAsync(new List<PostViewModel>
                {
                    new PostViewModel
                    {
                        //ApplicationUser = currentUser,
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

            var controller = new UserPostsController(mockService.Object, mockUserManager.Object);

            var result = await controller.Index(user.UserName, UserPostsFilter.Liked.ToString(), null);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsType<UserPostsViewModel>(viewResult.Model);

            var viewModel = viewResult.Model as UserPostsViewModel;
            Assert.Single(viewModel.Posts);
        }

        [Fact]
        public async Task IndexShouldReturnCorrectViewModelForCreatedPosts()
        {
            var user = new ApplicationUser { UserName = "pesho" };
            var currentUser = new ApplicationUser { UserName = "gogo" };
            var mockService = new Mock<IUserPostsService>();
            mockService
                .Setup(x => x.ExtractCreatedPostsByUsername(user.UserName, currentUser))
                .ReturnsAsync(new List<PostViewModel>
                {
                    new PostViewModel
                    {
                        //ApplicationUser = currentUser,
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

            var controller = new UserPostsController(mockService.Object, mockUserManager.Object);

            var result = await controller.Index(user.UserName, UserPostsFilter.Created.ToString(), null);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsType<UserPostsViewModel>(viewResult.Model);

            var viewModel = viewResult.Model as UserPostsViewModel;
            Assert.Single(viewModel.Posts);
        }
    }
}