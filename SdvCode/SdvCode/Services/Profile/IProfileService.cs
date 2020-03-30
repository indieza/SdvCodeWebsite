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
        ApplicationUserViewModel ExtractUserInfo(string username, HttpContext httpContext);

        Task<ApplicationUser> FollowUser(string username, HttpContext httpContext);

        Task<ApplicationUser> UnfollowUser(string username, HttpContext httpContext);

        Task<List<UserCardViewModel>> GetAllUsers(HttpContext httpContext, string search);

        void DeleteActivity(HttpContext httpContext);

        Task<string> DeleteActivityById(HttpContext httpContext, int activityId);

        Task<bool> HasAdmin();

        void MakeYourselfAdmin(string username);

        Task<int> TakeCreatedPostsCountByUsername(string username);

        Task<int> TakeLikedPostsCountByUsername(string username);
    }
}