// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services
{
    using SdvCode.Models;
    using SdvCode.ViewModels.Users;

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