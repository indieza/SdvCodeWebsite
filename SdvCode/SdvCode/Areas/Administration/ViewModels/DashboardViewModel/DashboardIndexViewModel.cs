namespace SdvCode.Areas.Administration.ViewModels.DashboardViewModels
{
    public class DashboardIndexViewModel
    {
        public DashboardViewModel DashboardViewModel { get; set; }

        public CreateRoleInputModel CreateRole { get; set; }

        public AddUserInRoleInputModel AddUserInRole { get; set; }

        public RemoveUserFromRoleInputModel RemoveUserFromRole { get; set; }
    }
}