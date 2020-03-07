using SdvCode.Areas.Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public interface IAdministrationService
    {
        DashboardViewModel GetDashboardInformation();
    }
}