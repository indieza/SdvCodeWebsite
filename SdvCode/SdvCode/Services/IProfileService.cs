using SdvCode.Data.Models;
using SdvCode.ViewModels.Users;
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

        AllUsersViewModel GetAllUsers(string currentUserId);

        void DeleteActivity(string currentUserId);

        void DeleteActivityById(string currentUserId, int activityId);
    }
}