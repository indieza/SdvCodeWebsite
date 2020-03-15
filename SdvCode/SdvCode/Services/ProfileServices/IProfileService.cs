// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.ProfileServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users;

    public interface IProfileService
    {
        ApplicationUser ExtractUserInfo(string username, string currentUserId);

        Task<ApplicationUser> FollowUser(string username, string currentUserId);

        Task<ApplicationUser> UnfollowUser(string username, string currentUserId);

        AllUsersViewModel GetAllUsers(string currentUserId);

        void DeleteActivity(string currentUserId);

        Task<string> DeleteActivityById(string currentUserId, int activityId);

        Task<bool> HasAdmin();

        void MakeYourselfAdmin(string username);
    }
}