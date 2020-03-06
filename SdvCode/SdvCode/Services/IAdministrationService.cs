using SdvCode.ViewModels.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public interface IAdministrationService
    {
        DashboardViewModel GetDashboardInformation();
    }
}