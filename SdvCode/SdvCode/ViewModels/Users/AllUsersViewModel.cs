using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.ViewModels.Users
{
    public class AllUsersViewModel
    {
        public ICollection<UserCardViewModel> UsersCards { get; set; } = new HashSet<UserCardViewModel>();
    }
}