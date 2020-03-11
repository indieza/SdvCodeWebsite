using System.ComponentModel.DataAnnotations;

namespace SdvCode.ViewModels.Contacts
{
    public class ContactInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}