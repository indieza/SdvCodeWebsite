﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditChatSticker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatSticker.InputModels;

    public class EditChatStickerBaseModel
    {
        public ICollection<EditChatStickerViewModel> EditChatStickerViewModels { get; set; } =
            new HashSet<EditChatStickerViewModel>();

        public EditChatStickerInputModel EditChatStickerInputModel { get; set; } =
            new EditChatStickerInputModel();

        public ICollection<EditStickerStickerTypeViewModel> AllStikersTypes { get; set; } =
            new HashSet<EditStickerStickerTypeViewModel>();
    }
}