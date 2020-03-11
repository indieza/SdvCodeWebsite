namespace SdvCode.Areas.Administration.ViewModels.DashboardViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserInRoleInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }
    }
}