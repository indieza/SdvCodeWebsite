using Microsoft.AspNetCore.Identity;
using SdvCode.Areas.Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public interface IDashboardService
    {
        DashboardViewModel GetDashboardInformation();

        Task<IdentityResult> CreateRole(string role);

        Task<bool> IsAddedUserInRole(string inputRole, string inputUsername);
    }
}