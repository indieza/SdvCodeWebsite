// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.UserPenalties
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SdvCode.Models.User;

    public interface IUsersPenaltiesService
    {
        ICollection<string> GetAllBlockedUsers();

        Task<ICollection<string>> GetAllNotBlockedUsers();

        Task<bool> BlockUser(string username, ApplicationUser currentUser, string reasonToBeBlocked);

        Task<bool> UnblockUser(string username, ApplicationUser currentUser);

        Task<int> BlockAllUsers();

        Task<int> UnblockAllUsers();
    }
}