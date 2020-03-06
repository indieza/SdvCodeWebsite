using SdvCode.Data;
using SdvCode.Services;
using SdvCode.ViewModels.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext db;

        public AdministrationService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public DashboardViewModel GetDashboardInformation()
        {
            return new DashboardViewModel
            {
                TotalUsersCount = db.Users.Count(),
                TotalBannedUsers = 10,
                TotalBlogPosts = 12,
                TotalUsersInRole = 5
            };
        }
    }
}