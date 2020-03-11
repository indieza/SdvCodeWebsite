namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteUsersImages
{
    using System.Collections.Generic;

    public class DeleteUsersImagesViewModel
    {
        public DeleteImagesByUsernameInputModel DeleteUserImages { get; set; }

        public ICollection<string> Usernames { get; set; } = new HashSet<string>();
    }
}