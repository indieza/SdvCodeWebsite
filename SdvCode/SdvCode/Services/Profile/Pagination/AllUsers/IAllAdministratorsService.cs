// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Profile.Pagination.AllUsers
{
    using SdvCode.ViewModels.Users.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAllAdministratorsService
    {
        Task<List<UserCardViewModel>> ExtractAllUsers(string username, string search);
    }
}