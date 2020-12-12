// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatStickerType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.ViewModels;

    public interface IDeleteChatStickerTypeService
    {
        ICollection<DeleteChatStickerTypeViewModel> GetAllStickersTypes();

        List<string> GetStickersUrls(string stickerTypeId);

        Task<Tuple<bool, string>> DeleteChatStickerType(DeleteChatStickerTypeInputModel model);
    }
}