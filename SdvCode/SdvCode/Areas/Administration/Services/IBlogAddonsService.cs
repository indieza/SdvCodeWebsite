// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBlogAddonsService
    {
        Task<bool> CreateCategory(string name, string description);
        Task<bool> CreateTag(string name);
    }
}