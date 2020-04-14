// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SdvCode.Models.User;
    using SdvCode.ViewModels.Profile;
    using SdvCode.ViewModels.Users.ViewModels;

    public interface IProfileService
    {
        Task<ApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser user);

        Task<ApplicationUser> FollowUser(string username, ApplicationUser user);

        Task<ApplicationUser> UnfollowUser(string username, ApplicationUser user);

        Task DeleteActivity(ApplicationUser user);

        Task<string> DeleteActivityById(ApplicationUser user, int activityId);

        Task<bool> HasAdmin(ApplicationRole role);

        void MakeYourselfAdmin(string username);

        Task<int> TakeCreatedPostsCountByUsername(string username);

        Task<int> TakeLikedPostsCountByUsername(string username);

        Task<int> TakeCommentsCountByUsername(string username);

        Task<double> RateUser(ApplicationUser currentUser, string username, int rate);

        double ExtractUserRatingScore(string username);

        Task<int> GetLatestScore(ApplicationUser currentUser, string username);
    }
}