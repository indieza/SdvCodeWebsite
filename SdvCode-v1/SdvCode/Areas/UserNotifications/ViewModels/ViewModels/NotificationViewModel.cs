// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.ViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.UserNotifications.Models.Enums;
    using SdvCode.Models.User;

    public class NotificationViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Heading { get; set; }

        [Required]
        public NotificationStatus Status { get; set; }

        [Required]
        public ICollection<string> AllStatuses { get; set; } = new HashSet<string>();

        [Required]
        public string ImageUrl { get; set; }

        public string TargetFirstName { get; set; }

        public string TargetLastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string TargetUsername { get; set; }

        [Required]
        public string CreatedOn { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}