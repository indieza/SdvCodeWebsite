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
    using SdvCode.Areas.UserNotifications.Models;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext db;

        public NotificationService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<string> AddMessageNotification(string fromUsername, string toUsername, string message, string group)
        {
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var notification = new UserNotification
            {
                ApplicationUserId = fromUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = message,
                TargetUsername = toUser.UserName,
                Link = $"/PrivateChat/With/{fromUser.UserName}/Group/{group}",
                NotificationType = NotificationType.Message,
            };

            var targetNotifications = this.db.UserNotifications
                .Where(x => x.NotificationType == NotificationType.Message &&
                x.TargetUsername == toUser.UserName &&
                x.ApplicationUserId == fromUser.Id)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            if (targetNotifications.Count + 1 > GlobalConstants.MaxChatNotificationsPerUser)
            {
                targetNotifications = targetNotifications
                    .Skip(GlobalConstants.MaxChatNotificationsPerUser - 1)
                    .ToList();
                this.db.UserNotifications.RemoveRange(targetNotifications);
            }

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<bool> DeleteNotification(string username, string id)
        {
            var notification = await this.db.UserNotifications
                .FirstOrDefaultAsync(x => x.Id == id && x.TargetUsername == username);
            if (notification != null)
            {
                this.db.UserNotifications.Remove(notification);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> EditStatus(ApplicationUser currentUser, string newStatus, string id)
        {
            var notification = await this.db.UserNotifications
                .FirstOrDefaultAsync(x => x.Id == id && x.TargetUsername == currentUser.UserName);

            if (notification != null)
            {
                notification.Status = (NotificationStatus)Enum.Parse(typeof(NotificationStatus), newStatus);
                this.db.UserNotifications.Update(notification);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Tuple<List<NotificationViewModel>, bool>> GetUserNotifications(ApplicationUser currentUser, int count, int skip)
        {
            var result = new List<NotificationViewModel>();
            var notifications = this.db.UserNotifications
                .Where(x => x.TargetUsername == currentUser.UserName)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(count)
                .ToList();

            foreach (var notification in notifications)
            {
                var user = await this.db.Users
                           .FirstOrDefaultAsync(x => x.Id == notification.ApplicationUserId);

                NotificationViewModel item = this.ParseNotificationViewModel(notification, user);
                result.Add(item);
            }

            var notificationsCount = await this.GetUserNotificationsCount(currentUser.UserName);

            return Tuple.Create(result, notificationsCount > skip + count ? true : false);
        }

        public async Task<NotificationViewModel> GetNotificationById(string id)
        {
            var notification = await this.db.UserNotifications.FirstOrDefaultAsync(x => x.Id == id);

            var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == notification.ApplicationUserId);
            NotificationViewModel item = this.ParseNotificationViewModel(notification, user);

            return item;
        }

        public async Task<int> GetUserNotificationsCount(string userName)
        {
            var count = await this.db.UserNotifications
                    .CountAsync(x => x.TargetUsername == userName && x.Status == NotificationStatus.Unread);
            return count;
        }

        public async Task<string> UpdateMessageNotifications(string fromUsername, string username)
        {
            var fromUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == fromUsername);
            var toUser = await this.db.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (fromUser != null && toUser != null)
            {
                var notifications = this.db.UserNotifications
                    .Where(x => x.Status == NotificationStatus.Unread &&
                    x.TargetUsername == toUser.UserName &&
                    x.ApplicationUserId == fromUser.Id)
                    .ToList();

                foreach (var notification in notifications)
                {
                    await this.EditStatus(toUser, NotificationStatus.Read.ToString(), notification.Id);
                }

                return toUser.Id;
            }

            return string.Empty;
        }

        private string GetNotificationHeading(NotificationType notificationType, ApplicationUser user, string link)
        {
            string message = string.Empty;

            switch (notificationType)
            {
                case NotificationType.Message:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> send you a new <a href=\"{link}\" style=\"text-decoration: underline\">message</a>";
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

                case NotificationType.CreateNewBlogPost:
                    break;

                default:
                    break;
            }

            return message;
        }

        private NotificationViewModel ParseNotificationViewModel(UserNotification notification, ApplicationUser user)
        {
            var contentWithoutTags =
                Regex.Replace(notification.Text, "<.*?>", string.Empty);

            return new NotificationViewModel
            {
                Id = notification.Id,
                CreatedOn = notification.CreatedOn.ToLocalTime().ToString("dd-MMMM-yyyy HH:mm tt"),
                TargetFirstName = user.FirstName,
                TargetLastName = user.LastName,
                ImageUrl = user.ImageUrl,
                Heading = this.GetNotificationHeading(notification.NotificationType, user, notification.Link),
                Status = notification.Status,
                Text = contentWithoutTags.Length < 487 ?
                                contentWithoutTags :
                                $"{contentWithoutTags.Substring(0, 487)}...",
                TargetUsername = notification.TargetUsername,
                AllStatuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(x => x.ToString()).ToList(),
            };
        }
    }
}