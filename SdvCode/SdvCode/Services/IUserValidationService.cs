// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SdvCode.Models.User;

    public interface IUserValidationService
    {
        Task<bool> IsInCommentRole(ApplicationUser user, string id);

        Task<bool> IsPostBlockedOrPending(string id);
    }
}