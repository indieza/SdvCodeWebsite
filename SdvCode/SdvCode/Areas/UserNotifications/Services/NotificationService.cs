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
                var targetUser = await this.db.Users
                        .FirstOrDefaultAsync(x => x.UserName == notification.TargetUsername);

                NotificationViewModel item = this.ParseNotificationViewModel(notification, user, targetUser);
                result.Add(item);
            }

            var notificationsCount = await this.GetUserNotificationsCount(currentUser.UserName);

            return Tuple.Create(result, notificationsCount > skip + count);
        }

        public async Task<NotificationViewModel> GetNotificationById(string id)
        {
            var notification = await this.db.UserNotifications.FirstOrDefaultAsync(x => x.Id == id);

            var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == notification.ApplicationUserId);
            var targetUser = await this.db.Users
                .FirstOrDefaultAsync(x => x.UserName == notification.TargetUsername);
            NotificationViewModel item = this.ParseNotificationViewModel(notification, user, targetUser);

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

        public async Task<string> AddBlogPostNotification(ApplicationUser toUser, ApplicationUser fromUser, string shortContent, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = fromUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = shortContent,
                TargetUsername = toUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.CreateNewBlogPost,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddApprovedPostNotification(ApplicationUser targetUser, ApplicationUser currentUser, string shortContent, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = currentUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = shortContent,
                TargetUsername = targetUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.ApprovedPost,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddUnbannPostNotification(ApplicationUser targetUser, ApplicationUser currentUser, string shortContent, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = currentUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = shortContent,
                TargetUsername = targetUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.UnbannedPost,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddBannPostNotification(ApplicationUser targetUser, ApplicationUser currentUser, string shortContent, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = currentUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = shortContent,
                TargetUsername = targetUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.BannedPost,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddPostToFavoriteNotification(ApplicationUser targetUser, ApplicationUser currentUser, string shortContent, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = currentUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = shortContent,
                TargetUsername = targetUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.AddToFavorite,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddProfileRatingNotification(ApplicationUser user, ApplicationUser currentUser, int rate)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = currentUser.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = $"{currentUser.UserName.ToUpper()} rate your profile with {rate} stars",
                TargetUsername = user.UserName,
                Link = $"/Profile/{user.UserName}",
                NotificationType = NotificationType.RateProfile,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddCommentPostNotification(ApplicationUser toUser, ApplicationUser user, string content, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = content,
                TargetUsername = toUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.CommentPost,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddCommentReplyNotification(ApplicationUser toUser, ApplicationUser user, string content, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = content,
                TargetUsername = toUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.ReplyToComment,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<string> AddApprovedCommentNotification(ApplicationUser toUser, ApplicationUser user, string content, string postId)
        {
            var notification = new UserNotification
            {
                ApplicationUserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                Status = NotificationStatus.Unread,
                Text = content,
                TargetUsername = toUser.UserName,
                Link = $"/Blog/Post/{postId}",
                NotificationType = NotificationType.ApprovedComment,
            };

            this.db.UserNotifications.Add(notification);
            await this.db.SaveChangesAsync();
            return notification.Id;
        }

        private string GetNotificationHeading(NotificationType notificationType, ApplicationUser user, string link)
        {
            string message = string.Empty;

            switch (notificationType)
            {
                case NotificationType.Message:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> send you a new <a href=\"{link}\" style=\"text-decoration: underline\">message</a>.";
                    break;

                case NotificationType.ApprovedPost:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> approved your <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a>.";
                    break;

                case NotificationType.BannedPost:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> banned your <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a>.";
                    break;

                case NotificationType.UnbannedPost:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> unbanned your <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a>.";
                    break;

                case NotificationType.AddToFavorite:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> added your <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a> to his <a href=\"/Profile/{user.UserName}/Favorites\" style=\"text-decoration: underline\">favorite list</a>.";
                    break;

                case NotificationType.RateProfile:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> rate your <a href=\"{link}\" style=\"text-decoration: underline\">profile</a>.";
                    break;

                case NotificationType.CreateNewBlogPost:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> create a new <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a>.";
                    break;

                case NotificationType.CommentPost:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> add new <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a> comment.";
                    break;

                case NotificationType.ReplyToComment:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> reply to <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a> comment.";
                    break;

                case NotificationType.ApprovedComment:
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> approved <a href=\"{link}\" style=\"text-decoration: underline\">blog post</a> comment.";
                    break;

                default:
                    break;
            }

            return message;
        }

        private NotificationViewModel ParseNotificationViewModel(UserNotification notification, ApplicationUser user, ApplicationUser targetUser)
        {
            var contentWithoutTags =
                Regex.Replace(notification.Text, "<.*?>", string.Empty);

            return new NotificationViewModel
            {
                Id = notification.Id,
                CreatedOn = notification.CreatedOn.ToLocalTime().ToString("dd-MMMM-yyyy HH:mm tt"),
                TargetFirstName = targetUser.FirstName,
                TargetLastName = targetUser.LastName,
                ImageUrl = user.ImageUrl,
                Heading = this.GetNotificationHeading(notification.NotificationType, user, notification.Link),
                Status = notification.Status,
                Text = contentWithoutTags.Length < 487 ?
                                contentWithoutTags :
                                $"{contentWithoutTags.Substring(0, 487)}...",
                TargetUsername = targetUser.UserName,
                AllStatuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(x => x.ToString()).ToList(),
            };
        }
    }
}