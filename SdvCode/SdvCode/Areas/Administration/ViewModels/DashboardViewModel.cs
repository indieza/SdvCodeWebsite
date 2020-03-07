using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsersCount { get; set; }

        public int TotalBlogPosts { get; set; }

        public int TotalBannedUsers { get; set; }

        public int TotalUsersInRole { get; set; }

        public ICollection<string> Usernames { get; set; } = new HashSet<string>();
    }
}