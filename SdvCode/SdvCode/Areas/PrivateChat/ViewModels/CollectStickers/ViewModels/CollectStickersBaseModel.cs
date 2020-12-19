// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.ViewModels.CollectStickers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CollectStickersBaseModel
    {
        public ICollection<CollectStickersStickerTypeViewModel> AllStickerTypes { get; set; } =
            new HashSet<CollectStickersStickerTypeViewModel>();
    }
}