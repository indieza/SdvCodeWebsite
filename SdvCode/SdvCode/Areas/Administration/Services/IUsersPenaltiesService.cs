using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public interface IUsersPenaltiesService
    {
        ICollection<string> GetAllBlockedUsers();

        ICollection<string> GetAllNotBlockedUsers();

        Task<bool> BlockUser(string username);

        Task<bool> UnblockUser(string username);

        Task<int> BlockAllUsers();
        Task<int> UnblockAllUsers();
    }
}