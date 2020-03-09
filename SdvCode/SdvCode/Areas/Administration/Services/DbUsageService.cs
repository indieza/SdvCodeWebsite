using SdvCode.Data;
using SdvCode.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public class DbUsageService : IDbUsageService
    {
        private readonly ApplicationDbContext db;

        public DbUsageService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> RemoveActivitiesByName(UserActionsType actionValue)
        {
            var allActions = this.db.UserActions.Where(x => x.Action == actionValue).ToList();

            if (allActions.Count() == 0)
            {
                return false;
            }

            this.db.UserActions.RemoveRange(allActions);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<int> RemoveAllActivities()
        {
            var allActivities = this.db.UserActions.ToList();
            int count = allActivities.Count();

            if (count > 0)
            {
                this.db.UserActions.RemoveRange(allActivities);
                await this.db.SaveChangesAsync();
            }

            return count;
        }
    }
}