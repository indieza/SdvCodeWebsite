// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditChatStickerType.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.InputModels;

    public class EditChatStickerTypeBaseModel
    {
        public ICollection<EditChatStickerTypeViewModel> EditChatStickerTypeViewModels { get; set; } =
            new HashSet<EditChatStickerTypeViewModel>();

        public EditChatStickerTypeInputModel EditChatStickerTypeInputModel { get; set; } =
            new EditChatStickerTypeInputModel();
    }
}