namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteUsersImages
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteImagesByUsernameInputModel
    {
        [Required]
        public string Username { get; set; }
    }
}