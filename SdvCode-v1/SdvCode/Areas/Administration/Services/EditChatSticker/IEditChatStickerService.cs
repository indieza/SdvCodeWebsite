﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditChatSticker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatSticker.ViewModels;

    public interface IEditChatStickerService
    {
        ICollection<EditStickerStickerTypeViewModel> GetAllStikersTypes();

        ICollection<EditChatStickerViewModel> GetAllStickers();

        Task<GetEditChatStickerDataViewModel> GetStickerById(string stickerId);

        Task<Tuple<bool, string>> EditSticker(EditChatStickerInputModel model);
    }
}