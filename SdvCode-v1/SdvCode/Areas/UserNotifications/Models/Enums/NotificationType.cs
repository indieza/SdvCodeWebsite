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
        BannedPost = 3,
        UnbannedPost = 4,
        AddToFavorite = 5,
        RateProfile = 6,
        CreateNewBlogPost = 7,
        CommentPost = 8,
        ReplyToComment = 9,
        ApprovedComment = 10,
        BannedProfile = 11,
        UnbannedProfile = 12,
    }
}