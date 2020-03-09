using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Models.Enums
{
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
        EditPersonalData = 7
    }
}