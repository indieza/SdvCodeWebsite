// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Contact
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using SdvCode.ViewModels.Contacts;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class ContactService : IContactService
    {
        private readonly IConfiguration configuration;

        public ContactService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(ContactInputModel model)
        {
            this.Execute(model).Wait();
        }

        private async Task Execute(ContactInputModel model)
        {
            var apiKey = this.configuration.GetSection("SendGrid:ApiKey").Value;
            var client = new SendGridClient(apiKey);

            var message = new SendGridMessage()
            {
                From = new EmailAddress(model.Email, model.Name),
                Subject = model.Subject,
                PlainTextContent = model.Message,
                HtmlContent = $"<strong>Hello, SDV Code Administrators!</strong><br />{model.Message}",
            };

            message.AddTo(new EmailAddress("sdvcodeproject@gmail.com", "Test User"));
            var response = await client.SendEmailAsync(message);
        }
    }
}