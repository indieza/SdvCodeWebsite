using Microsoft.AspNetCore.Identity;
using SdvCode.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SdvCode.Services
{
    public interface IHomeService
    {
        int GetRegisteredUsersCount();

        Task<IdentityResult> CreateRole(string role);

        ICollection<ApplicationUser> GetAllAdministrators();
    }
}