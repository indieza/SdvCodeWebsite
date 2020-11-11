// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AllEmojis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AllEmojisViewModels.ViewModels;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public interface IAllEmojisService
    {
        Dictionary<EmojiType, ICollection<EmojiViewModel>> GetAllEmojis();
    }
}