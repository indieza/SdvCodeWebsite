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
        Task<ICollection<NotificationViewModel>> GetAllNotifications(ApplicationUser currentUser);
    }
}