// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.ViewModels.EditChatTheme.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.InputModels;

    public class EditChatThemeBaseModel
    {
        public ICollection<EditChatThemeViewModel> EditChatThemeViewModels { get; set; } =
            new HashSet<EditChatThemeViewModel>();

        public EditChatThemeInputModel EditChatThemeInput { get; set; } = new EditChatThemeInputModel();
    }
}