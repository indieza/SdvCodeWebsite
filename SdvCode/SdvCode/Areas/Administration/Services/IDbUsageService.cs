using SdvCode.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public interface IDbUsageService
    {
        Task<bool> RemoveActivitiesByName(UserActionsType actionValue);

        Task<int> RemoveAllActivities();
        ICollection<string> GetAllUsernames();
        Task<bool> DeleteUserImagesByUsername(string username);
        Task<int> DeleteAllUsersImages();
    }
}