namespace SdvCode.Tests.Tag.Controller
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
    using SdvCode.Services.Tag;
    using SdvCode.ViewModels.Post.ViewModels;
    using SdvCode.ViewModels.Tag;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class TagIndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModel()
        {
            var currentUser = new ApplicationUser { UserName = "gogo" };
            var tag = new Tag { Id = Guid.NewGuid().ToString() };
            var mockService = new Mock<ITagService>();
            mockService.Setup(x => x.ExtractPostsByTagId(tag.Id, currentUser))
                .ReturnsAsync(new List<PostViewModel>
                {
                   new PostViewModel
                   {
                       Id = Guid.NewGuid().ToString(),
                       Title = "Test Title",
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

            var controller = new TagController(mockService.Object, mockUserManager.Object);

            var result = await controller.Index(tag.Id, null);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsType<TagViewModel>(viewResult.Model);

            var viewModel = viewResult.Model as TagViewModel;
            Assert.Single(viewModel.Posts);
        }
    }
}