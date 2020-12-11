// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditChatStickerType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.ViewModels;

    public interface IEditChatStickerTypeService
    {
        ICollection<EditChatStickerTypeViewModel> GetAllChatStickerTypes();

        Task<GetEditChaStickerTypeDataViewModel> GetEmojiById(string stickerTypeId);

        Task<Tuple<bool, string>> EditStickerType(EditChatStickerTypeInputModel model);
    }
}