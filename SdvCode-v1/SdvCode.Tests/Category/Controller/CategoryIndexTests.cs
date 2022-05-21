namespace SdvCode.Tests.Category.Controller
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SdvCode.Controllers;
    using SdvCode.Models.Blog;
    using SdvCode.Models.User;
    using SdvCode.Services.Category;
    using SdvCode.ViewModels.Category;

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class CategoryIndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModelForLikedPosts()
        {
            var currentUser = new ApplicationUser { UserName = "gogo" };
            var category = new Category { Id = Guid.NewGuid().ToString() };
            var mockService = new Mock<ICategoryService>();
            //mockService
            //    .Setup(x => x.ExtractPostsByCategoryId(category.Id, currentUser))
            //    .ReturnsAsync(new List<PostViewModel>
            //    {
            //        new PostViewModel
            //        {
            //            //ApplicationUser = currentUser,
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

            var controller = new CategoryController(mockService.Object, mockUserManager.Object);

            var result = await controller.Index(category.Id, null);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            //Assert.IsType<CategoryViewModel>(viewResult.Model);

            //var viewModel = viewResult.Model as CategoryViewModel;
            //Assert.Single(viewModel.Posts);
        }
    }
}