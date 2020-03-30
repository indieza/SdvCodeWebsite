namespace SdvCode.Tests.Profile.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SdvCode.Controllers;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services.Profile;
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
            mockService.Setup(x => x.DeleteActivityById(new DefaultHttpContext(), 2)).ReturnsAsync("Follow");
        }
    }
}