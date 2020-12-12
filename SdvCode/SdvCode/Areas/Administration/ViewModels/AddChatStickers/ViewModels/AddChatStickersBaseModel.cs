// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.AddChatStickers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AddChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.InputModels;

    public class AddChatStickersBaseModel
    {
        public ICollection<AddChatStickersViewModel> AddChatStickersViewModels { get; set; } =
            new HashSet<AddChatStickersViewModel>();

        public AddChatStickersInputModel AddChatStickersInputModel { get; set; } =
            new AddChatStickersInputModel();
    }
}