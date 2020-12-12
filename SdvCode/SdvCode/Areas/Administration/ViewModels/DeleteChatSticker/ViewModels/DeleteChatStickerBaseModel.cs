// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeOpenXml.ConditionalFormatting;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.InputModels;

    public class DeleteChatStickerBaseModel
    {
        public ICollection<DeleteChatStickerViewModel> DeleteChatStickerViewModel { get; set; } =
            new HashSet<DeleteChatStickerViewModel>();

        public DeleteChatStickerInputModel DeleteChatStickerInputModel { get; set; } =
            new DeleteChatStickerInputModel();
    }
}