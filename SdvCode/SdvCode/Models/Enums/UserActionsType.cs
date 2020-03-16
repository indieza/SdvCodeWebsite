// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum UserActionsType
    {
        [Display(Name = "Follow")]
        Follow = 1,
        [Display(Name = "Followed")]
        Followed = 2,
        [Display(Name = "Unfollow")]
        Unfollow = 3,
        [Display(Name = "Unfollowed")]
        Unfollowed = 4,
        [Display(Name = "Change Cover Image")]
        ChangeCoverImage = 5,
        [Display(Name = "Change Profile Picture")]
        ChangeProfilePicture = 6,
        [Display(Name = "Edit Personal Data")]
        EditPersonalData = 7,
        [Display(Name = "Create Post")]
        CreatePost = 8,
    }
}