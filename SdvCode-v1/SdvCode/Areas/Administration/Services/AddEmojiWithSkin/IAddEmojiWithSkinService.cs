// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmojiWithSkin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddEmojiWithSkin.InputModels;

    public interface IAddEmojiWithSkinService
    {
        Task<string> AddEmojiWithSkin(AddEmojiWithSkinInputModel model);
    }
}