// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditEmojiPosition.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class EditEmojiPositionViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Position { get; set; }

        public EmojiType EmojiType { get; set; }
    }
}