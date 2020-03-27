namespace SdvCode.Tests.Contact.Controller
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using SdvCode.Controllers;
    using SdvCode.Services.Contact;
    using SdvCode.ViewModels.Contacts;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    public class ContactIndexTests
    {
        [Fact]
        public void IndexShouldReturnCorrectViewModel()
        {
            var mockService = new Mock<IContactService>();

            var controller = new ContactController(mockService.Object);

            var getResult = controller.Index();
            Assert.IsType<ViewResult>(getResult);
        }

        [Fact]
        public void TestInvalidInputModel()
        {
            var mockService = new Mock<IContactService>();

            var controller = new ContactController(mockService.Object);
            controller.ModelState.AddModelError("test", "test");

            var postResult = controller.Index(new ContactInputModel());
            Assert.IsType<ViewResult>(postResult);
        }

        [Fact]
        public void TestValidInputModel()
        {
            var model = new ContactInputModel()
            {
                Name = "Pesho",
                Email = "pesho_123@gamil.com",
                Subject = "Test subject",
                Message = "Hello from pesho's city",
            };

            var mockService = new Mock<IContactService>();
            mockService.Setup(x => x.SendEmail(model));

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new ContactController(mockService.Object)
            {
                TempData = tempData,
            };

            var postResult = controller.Index(model);

            Assert.IsType<RedirectToPageResult>(postResult);
            Assert.True(controller.TempData.ContainsKey("Success"));
            Assert.Equal(
                "Your message has been sent. Be patient you will receive a reply within 1 day.",
                controller.TempData["Success"]);
        }
    }
}