using Microsoft.AspNetCore.Identity;
using SdvCode.Areas.Administration.Models.Enums;
using SdvCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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