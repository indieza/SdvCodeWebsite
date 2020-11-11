// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditEmojiPosition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPosition.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPosition.ViewModels;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public interface IEditEmojiPositionService
    {
        ICollection<EditEmojiPositionViewModel> GetAllEmojisByType(EmojiType emojiType);

        Task<int> EditEmojisPosition(EditEmojiPositionInputModel[] allEmojis);
    }
}