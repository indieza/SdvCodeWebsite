using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteUsersImages
{
    public class DeleteUsersImagesViewModel
    {
        public DeleteImagesByUsernameInputModel DeleteUserImages { get; set; }

        public ICollection<string> Usernames { get; set; } = new HashSet<string>();
    }
}