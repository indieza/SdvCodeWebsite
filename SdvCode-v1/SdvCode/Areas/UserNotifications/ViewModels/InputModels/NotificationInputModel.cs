﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.ViewModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Models.User;

    public class NotificationInputModel
    {
        public string Id { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public NotificationStatus Status { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MaxLength(20)]
        public string TargetUsername { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);
    }
}