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
        [Display(Name = "Like Post")]
        LikePost = 9,
        [Display(Name = "Unlike Post")]
        UnlikePost = 10,
        [Display(Name = "Unliked Post")]
        UnlikedPost = 11,
        [Display(Name = "Liked Post")]
        LikedPost = 12,
        [Display(Name = "Liked Own Post")]
        LikeOwnPost = 13,
        [Display(Name = "Unlike Own Post")]
        UnlikeOwnPost = 14,
        [Display(Name = "Delete Own Post")]
        DeleteOwnPost = 15,
        [Display(Name = "Delete Post")]
        DeletePost = 16,
        [Display(Name = "Deleted Post")]
        DeletedPost = 17,
        [Display(Name = "Edit Post")]
        EditPost = 18,
        [Display(Name = "Edited Post")]
        EditedPost = 19,
        [Display(Name = "Edit Own Post")]
        EditOwnPost = 20,
    }
}