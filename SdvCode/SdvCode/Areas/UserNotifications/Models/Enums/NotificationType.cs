// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.UserNotifications.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum NotificationType
    {
        Message = 1,
        ApprovedPost = 2,
        Comment = 3,
        BannedPost = 4,
        UnbannedPost = 5,
        AddToFavorite = 6,
        RateProfile = 7,
        CreateNewBlogPost = 8,
    }
}