using SdvCode.Models;
using SdvCode.ViewModels.Users;

namespace SdvCode.Services
{
    public interface IProfileService
    {
        ApplicationUser ExtractUserInfo(string username, string currentUserId);

        ApplicationUser FollowUser(string username, string currentUserId);

        ApplicationUser UnfollowUser(string username, string currentUserId);

        AllUsersViewModel GetAllUsers(string currentUserId);

        void DeleteActivity(string currentUserId);

        string DeleteActivityById(string currentUserId, int activityId);

        bool HasAdmin();

        void MakeYourselfAdmin(string username);
    }
}