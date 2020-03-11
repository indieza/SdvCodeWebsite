using System.Collections.Generic;

namespace SdvCode.ViewModels.Users
{
    public class AllUsersViewModel
    {
        public ICollection<UserCardViewModel> UsersCards { get; set; } = new HashSet<UserCardViewModel>();
    }
}