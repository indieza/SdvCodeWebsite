using SdvCode.ViewModels.Contacts;

namespace SdvCode.Services
{
    public interface IContactsService
    {
        void SendEmail(ContactInputModel model);
    }
}