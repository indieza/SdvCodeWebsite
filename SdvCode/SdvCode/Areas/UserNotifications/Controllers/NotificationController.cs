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
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Areas.UserNotifications.ViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Hubs;
    using SdvCode.Models.User;

    [Area(GlobalConstants.NotificationsArea)]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotificationService notificationService;
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationController(
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext)
        {
            this.userManager = userManager;
            this.notificationService = notificationService;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            ICollection<NotificationViewModel> notifications =
                await this.notificationService.GetUserNotifications(currentUser, GlobalConstants.NotificationOnClick, 0);

            return this.View(notifications);
        }

        [HttpPost]
        [Route("/UserNotifications/Notification/EditStatus")]
        public async Task<bool> EditStatus(string newStatus, string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isEdited = await this.notificationService.EditStatus(currentUser, newStatus, id);
            await this.ChangeNotificationCounter(isEdited, currentUser);
            return isEdited;
        }

        [HttpPost]
        [Route("/UserNotifications/Notification/DeleteNotification")]
        public async Task<bool> DeleteNotification(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isDeleted = await this.notificationService.DeleteNotification(currentUser.UserName, id);
            await this.ChangeNotificationCounter(isDeleted, currentUser);
            return isDeleted;
        }

        [HttpGet]
        [Route("/UserNotifications/Notification/GetMoreNotitification")]
        public async Task<IActionResult> GetMoreNotitification(int skip)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var newNotifications = await this.notificationService.GetUserNotifications(currentUser, GlobalConstants.NotificationOnClick, skip);
            return new JsonResult(newNotifications);
        }

        private async Task ChangeNotificationCounter(bool isForChange, ApplicationUser user)
        {
            if (isForChange)
            {
                int count = await this.notificationService.GetUserNotificationsCount(user.UserName);
                await this.hubContext.Clients.User(user.Id).SendAsync("ReceiveNotification", count);
            }
        }
    }
}