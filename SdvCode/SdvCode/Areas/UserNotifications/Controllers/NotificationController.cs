// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Models.User;

    [Area(GlobalConstants.NotificationsArea)]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotificationService notificationService;

        public NotificationController(
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            this.userManager = userManager;
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ICollection<NotificationViewModel> notifications =
                await this.notificationService.GetAllNotifications(currentUser);

            return this.View(notifications);
        }
    }
}