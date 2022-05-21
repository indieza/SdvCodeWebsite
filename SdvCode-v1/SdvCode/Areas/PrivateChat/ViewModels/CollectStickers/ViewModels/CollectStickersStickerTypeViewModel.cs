// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.ViewModels.CollectStickers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CollectStickersStickerTypeViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool HaveIt { get; set; }

        public ICollection<CollectStickersStickerViewModel> AllStickers { get; set; } =
            new HashSet<CollectStickersStickerViewModel>();
    }
}