// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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