namespace SdvCode.Tests.Home.Controller
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SdvCode.Controllers;
    using SdvCode.Models.User;
    using SdvCode.Services.Home;
    using SdvCode.ViewModels.Home;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class HomeIndexTests
    {
        [Fact]
        public async Task IndexShouldReturnCorrectViewModel()
        {
            var mockService = new Mock<IHomeService>();
            mockService.Setup(x => x.GetRegisteredUsersCount()).Returns(2);
            mockService.Setup(x => x.GetAllAdministrators()).ReturnsAsync(new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    UserName = "pesho_peshev",
                },
                new ApplicationUser
                {
                    FirstName = "Simeon",
                    LastName = "Valev",
                    UserName ="indieza",
                }
            });

            var controller = new HomeController(mockService.Object);

            var result = await controller.Index();
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsType<HomeViewModel>(viewResult.Model);

            var viewModel = viewResult.Model as HomeViewModel;
            Assert.Equal(2, viewModel.TotalRegisteredUsers);
            Assert.Equal(2, viewModel.Administrators.Count);
            Assert.Equal("indieza", viewModel.Administrators.ToList()[1].UserName);
        }
    }
}