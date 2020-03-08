using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels
{
    public class DashboardIndexViewModel
    {
        public DashboardViewModel DashboardViewModel { get; set; }

        public CreateRoleInputModel CreateRoleInputModel { get; set; }

        public AddUserInRoleInputModel AddUserInRole { get; set; }
    }
}