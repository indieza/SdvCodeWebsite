﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatStickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.ViewModels;

    public interface IAddChatStickersService
    {
        ICollection<AddChatStickersViewModel> GetAllStickersTypes();

        Task<Tuple<bool, string>> AddChatStickers(AddChatStickersInputModel model);
    }
}