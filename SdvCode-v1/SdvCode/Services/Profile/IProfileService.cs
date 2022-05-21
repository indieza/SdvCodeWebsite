// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile
{
    using System.Threading.Tasks;

    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile.UserProfile;
    using SdvCode.ViewModels.Users.ViewModels;

    public interface IProfileService
    {
        Task<ProfileApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser user);

        Task<ApplicationUser> FollowUser(string username, ApplicationUser user);

        Task<ApplicationUser> UnfollowUser(string username, ApplicationUser user);

        Task DeleteActivity(ApplicationUser user);

        Task<string> DeleteActivityById(ApplicationUser user, string activityId);

        Task<bool> HasAdmin(ApplicationRole role);

        Task<bool> HasAdministrator();

        void MakeYourselfAdmin(string username);

        Task<double> RateUser(ApplicationUser currentUser, string username, int rate);

        double ExtractUserRatingScore(string username);

        Task<int> GetLatestScore(ApplicationUser currentUser, string username);

        Task<bool> IsUserExist(string username);

        Task ChangeActionStatus(string username, string id, string status);
    }
}