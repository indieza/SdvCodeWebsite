using System.Collections;
using System.Collections.Generic;

namespace SdvCode.Areas.Administration.ViewModels.UsersPenalties
{
    public class UsersPenaltiesViewModel
    {
        public ICollection<string> BlockedUsernames { get; set; } = new HashSet<string>();

        public ICollection<string> NotBlockedUsernames { get; set; } = new HashSet<string>();
    }
}