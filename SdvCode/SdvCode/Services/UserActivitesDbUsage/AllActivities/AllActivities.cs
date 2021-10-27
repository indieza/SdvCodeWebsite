// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.UserActivitesDbUsage.AllActivities
{
    using System.Linq;

    using SdvCode.Data;
    using SdvCode.Models.Enums;

    public class AllActivities : IAllActivities
    {
        private readonly ApplicationDbContext db;

        public AllActivities(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void DeleteAllActivites()
        {
            var target = this.db.UserActions
                .Where(x => x.ActionStatus != UserActionStatus.Pinned)
                .ToList();
            this.db.RemoveRange(target);
            this.db.SaveChanges();
        }
    }
}