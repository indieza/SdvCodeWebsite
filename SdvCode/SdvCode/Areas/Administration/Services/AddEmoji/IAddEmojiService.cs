// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddEmojiViewModels.InputModels;

    public interface IAddEmojiService
    {
        Task<Tuple<bool, string>> AddEmoji(AddEmojiInputModel model);
    }
}