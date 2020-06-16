// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Models.User;

    public interface INotificationService
    {
        Task<Tuple<List<NotificationViewModel>, bool>> GetUserNotifications(ApplicationUser currentUser, int count, int skip);

        Task<bool> EditStatus(ApplicationUser currentUser, string newStatus, string id);

        Task<bool> DeleteNotification(string username, string id);

        Task<int> GetUserNotificationsCount(string userName);

        Task<NotificationViewModel> GetNotificationById(string id);

        Task<string> AddMessageNotification(string fromUsername, string toUsername, string message, string group);

        Task<string> UpdateMessageNotifications(string fromUsername, string username);

        Task<string> AddBlogPostNotification(ApplicationUser toUser, ApplicationUser fromUser, string shortContent, string postId);
    }
}