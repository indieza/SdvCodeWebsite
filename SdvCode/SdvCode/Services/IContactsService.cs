using SdvCode.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public interface IContactsService
    {
        void SendEmail(ContactInputModel model);
    }
}