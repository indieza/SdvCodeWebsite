// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext db;

        public NotificationService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<NotificationViewModel>> GetAllNotifications(ApplicationUser currentUser)
        {
            var result = new List<NotificationViewModel>();
            var notifications = this.db.UserNotifications.OrderByDescending(x => x.CreatedOn).ToList();

            foreach (var notification in notifications)
            {
                var item = new NotificationViewModel
                {
                    Id = notification.Id,
                    CreatedOn = notification.CreatedOn,
                    ApplicationUser = await this.db.Users
                    .FirstOrDefaultAsync(x => x.Id == notification.ApplicationUserId),
                    ApplicationUserId = notification.ApplicationUserId,
                    NotificationType = notification.NotificationType,
                    TargetUsername = notification.TargetUsername,
                    Link = string.Empty, // TODO::
                };
                result.Add(item);
            }

            return result;
        }
    }
}