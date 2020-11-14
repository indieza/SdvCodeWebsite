// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmojis
{
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAddEmojisService
    {
        Task<string> AddEmojis(AddEmojisInputModel model);
    }
}