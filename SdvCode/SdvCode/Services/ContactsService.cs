using Microsoft.Extensions.Configuration;
using SdvCode.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var mailBody = $@"Hello website owner,
            This is a new contact request from your website:
            Name: {model.Name}
            Subject: {model.Subject}
            Email: {model.Email}
            Message: {model.Message}
            Have a nice day bro!!!";

            SendMail(mailBody, model);
        }

        private void SendMail(string mailBody, ContactInputModel model)
        {
            var mailConfigurations = configuration.GetSection("MailLogging");
            string email = mailConfigurations.GetSection("Email").Value;
            string password = mailConfigurations.GetSection("Password").Value;

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(model.Email);
                message.To.Add(email);
                message.Subject = model.Subject;
                message.Body = mailBody;

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new NetworkCredential(email, password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}