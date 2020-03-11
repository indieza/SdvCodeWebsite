using Microsoft.Extensions.Configuration;
using SdvCode.ViewModels.Contacts;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IConfiguration configuration;

        public ContactsService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(ContactInputModel model)
        {
            Execute(model).Wait();
        }

        private async Task Execute(ContactInputModel model)
        {
            var apiKey = configuration.GetSection("SendGrid:ApiKey").Value;
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