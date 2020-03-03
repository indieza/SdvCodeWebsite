using SdvCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public interface IProfileService
    {
        ApplicationUser ExtractUserInfo(string username, string currentUserId);

        ApplicationUser FollowUser(string username, string currentUserId);

        ApplicationUser UnfollowUser(string username, string currentUserId);
    }
}