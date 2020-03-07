using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Models.Enums
{
    public enum UserActionsType
    {
        Follow = 1,
        Followed = 2,
        Unfollow = 3,
        Unfollowed = 4,
        ChangeCoverImage = 5,
        ChangeProfilePicture = 6,
        EditPersonalData = 7
    }
}