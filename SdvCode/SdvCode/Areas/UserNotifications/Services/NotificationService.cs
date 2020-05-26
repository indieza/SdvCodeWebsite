// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.UserNotifications.Models.Enums;
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
                var user = await this.db.Users
                           .FirstOrDefaultAsync(x => x.Id == notification.ApplicationUserId);
                var contentWithoutTags =
                    Regex.Replace(notification.Text, "<.*?>", string.Empty);
                var item = new NotificationViewModel
                {
                    Id = notification.Id,
                    CreatedOn = notification.CreatedOn,
                    ApplicationUser = user,
                    ApplicationUserId = notification.ApplicationUserId,
                    NotificationHeading = this.GetNotificationHeading(notification.NotificationType, user),
                    Status = notification.Status,
                    Text = contentWithoutTags.Length < 487 ?
                        contentWithoutTags :
                        $"{contentWithoutTags.Substring(0, 487)}...",
                    TargetUsername = notification.TargetUsername,
                    Link = string.Empty, // TODO::
                };
                result.Add(item);
            }

            return result;
        }

        private string GetNotificationHeading(NotificationType notificationType, ApplicationUser user)
        {
            string message = string.Empty;

            switch (notificationType)
            {
                case NotificationType.Message:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> send you a new message";
                    break;

                case NotificationType.Followed:
                    break;

                case NotificationType.Liked:
                    break;

                case NotificationType.Unfollowed:
                    break;

                case NotificationType.Unliked:
                    break;

                case NotificationType.AddToFavorite:
                    break;

                case NotificationType.RemoveFromFavorite:
                    break;

                default:
                    break;
            }

            return message;
        }
    }
}