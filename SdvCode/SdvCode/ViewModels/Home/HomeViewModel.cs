using SdvCode.Models;
using System.Collections.Generic;

namespace SdvCode.ViewModels.Home
{
    public class HomeViewModel
    {
        public int TotalRegisteredUsers { get; set; }

        public ICollection<ApplicationUser> Administrators { get; set; } = new HashSet<ApplicationUser>();
    }
}