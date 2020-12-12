// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.InputModels;

    public class DeleteChatStickerTypeBaseModel
    {
        public ICollection<DeleteChatStickerTypeViewModel> DeleteChatStickerTypeViewModel { get; set; } =
            new HashSet<DeleteChatStickerTypeViewModel>();

        public DeleteChatStickerTypeInputModel DeleteChatStickerTypeInputModel { get; set; } =
            new DeleteChatStickerTypeInputModel();
    }
}