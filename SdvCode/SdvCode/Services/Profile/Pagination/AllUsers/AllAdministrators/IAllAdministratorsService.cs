// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers.AllAdministrators
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SdvCode.ViewModels.Users.ViewModels;

    public interface IAllAdministratorsService
    {
        Task<List<AllUsersUserCardViewModel>> ExtractAllUsers(string username, string search);
    }
}