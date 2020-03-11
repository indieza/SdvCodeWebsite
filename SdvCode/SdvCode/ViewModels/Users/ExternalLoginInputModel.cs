using System.ComponentModel.DataAnnotations;

namespace SdvCode.ViewModels.Users
{
    public class ExternalLoginInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}