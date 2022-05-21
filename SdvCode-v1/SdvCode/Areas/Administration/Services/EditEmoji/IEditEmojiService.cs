﻿// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.ViewModels;

    public interface IEditEmojiService
    {
        Task<GetEditEmojiDataViewModel> GetEmojiById(string emojiId);

        ICollection<EditEmojiViewModel> GetAllEmojis();

        Task<Tuple<bool, string>> EditEmoji(EditEmojiInputModel model);
    }
}