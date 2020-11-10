// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmojiViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmojiViewModels.ViewModels;

    public interface IDeleteEmojiService
    {
        ICollection<DeleteEmojiViewModel> GetAllEmojis();

        Task<Tuple<bool, string>> DeleteEmoji(DeleteEmojiInputModel model);
    }
}