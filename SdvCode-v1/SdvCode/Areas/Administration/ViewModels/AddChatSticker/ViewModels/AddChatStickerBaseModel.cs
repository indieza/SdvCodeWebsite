// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddChatSticker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.InputModels;

    public class AddChatStickerBaseModel
    {
        public ICollection<AddChatStickerViewModel> AddChatStickerViewModels { get; set; } =
            new HashSet<AddChatStickerViewModel>();

        public AddChatStickerInputModel AddChatStickerInputModel { get; set; } = new AddChatStickerInputModel();
    }
}