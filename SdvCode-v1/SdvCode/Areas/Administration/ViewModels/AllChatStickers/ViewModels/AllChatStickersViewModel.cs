// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AllChatStickers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllChatStickersViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public string Url { get; set; }

        public ICollection<AllChatStickersStickerViewModel> AllStickerst { get; set; } =
            new HashSet<AllChatStickersStickerViewModel>();
    }
}