namespace SdvCode.Areas.Administration.ViewModels.UsersPenalties
{
    using System.Collections.Generic;

    public class UsersPenaltiesViewModel
    {
        public ICollection<string> BlockedUsernames { get; set; } = new HashSet<string>();

        public ICollection<string> NotBlockedUsernames { get; set; } = new HashSet<string>();
    }
}