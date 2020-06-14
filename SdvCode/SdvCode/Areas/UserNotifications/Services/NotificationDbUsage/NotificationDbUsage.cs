// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Services.NotificationDbUsage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Data;

    public class NotificationDbUsage : INotificationDbUsage
    {
        private readonly ApplicationDbContext db;

        public NotificationDbUsage(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task DeleteNotifications()
        {
            var target = this.db.UserNotifications
                .Where(x => x.Status == NotificationStatus.Read)
                .ToList();
            this.db.UserNotifications.RemoveRange(target);
            await this.db.SaveChangesAsync();
        }
    }
}