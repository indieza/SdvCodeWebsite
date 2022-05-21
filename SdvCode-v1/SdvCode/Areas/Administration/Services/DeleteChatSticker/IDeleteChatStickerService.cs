// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatSticker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.ViewModels;

    public interface IDeleteChatStickerService
    {
        ICollection<DeleteChatStickerViewModel> GetAllStickers();

        Task<string> GetStickerUrl(string stickerId);

        Task<Tuple<bool, string>> DeleteChatSticker(DeleteChatStickerInputModel model);
    }
}