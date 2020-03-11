namespace SdvCode.Areas.Administration.ViewModels.DashboardViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveUserFromRoleInputModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Username { get; set; }
    }
}