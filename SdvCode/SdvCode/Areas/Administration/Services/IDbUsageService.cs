namespace SdvCode.Areas.Administration.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SdvCode.Models.Enums;

    public interface IDbUsageService
    {
        Task<bool> RemoveActivitiesByName(UserActionsType actionValue);

        Task<int> RemoveAllActivities();

        ICollection<string> GetAllUsernames();

        Task<bool> DeleteUserImagesByUsername(string username);

        Task<int> DeleteAllUsersImages();
    }
}