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
        Task<bool> IsInPostRole(ApplicationUser user, string id);

        Task<bool> IsInCommentRole(ApplicationUser user, string id);

        Task<bool> IsPostApproved(string id, ApplicationUser user);

        Task<bool> IsPostBlockedOrPending(string id);

        Task<bool> IsPostBlocked(string id, ApplicationUser user);
    }
}