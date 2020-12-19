// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.CollectStickers
{
    using SdvCode.Areas.PrivateChat.ViewModels.CollectStickers.ViewModels;
    using SdvCode.Models.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICollectStickersService
    {
        ICollection<CollectStickersStickerTypeViewModel> GetAllStickers(ApplicationUser currentUser);
    }
}