// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.BannedUsers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Users.ViewModels;

    public interface IBannedUsersService
    {
        Task<List<AllUsersUserCardViewModel>> ExtractAllUsers(string username, string search);
    }
}