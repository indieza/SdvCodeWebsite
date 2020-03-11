using SdvCode.Models;

namespace SdvCode.ViewModels.Profiles
{
    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }
    }
}