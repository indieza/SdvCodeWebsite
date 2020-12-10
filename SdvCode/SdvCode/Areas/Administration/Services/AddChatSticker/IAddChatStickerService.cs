// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatSticker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.ViewModels;

    public interface IAddChatStickerService
    {
        ICollection<AddChatStickerViewModel> GetAllStickerTypes();

        Task<Tuple<bool, string>> AddNewSticker(AddChatStickerInputModel model);
    }
}