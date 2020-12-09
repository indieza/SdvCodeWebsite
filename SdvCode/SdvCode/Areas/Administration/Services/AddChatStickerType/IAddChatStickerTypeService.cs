// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatStickerType
{
    using SdvCode.Areas.Administration.ViewModels.AddChatStickerType.InputModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAddChatStickerTypeService
    {
        Task<Tuple<bool, string>> AddNewStickerType(AddChatStickerTypeInputModel model);
    }
}