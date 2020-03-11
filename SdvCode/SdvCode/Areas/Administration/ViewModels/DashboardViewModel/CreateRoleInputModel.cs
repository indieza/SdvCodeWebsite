namespace SdvCode.Areas.Administration.ViewModels.DashboardViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateRoleInputModel
    {
        [Required]
        public string Role { get; set; }
    }
}