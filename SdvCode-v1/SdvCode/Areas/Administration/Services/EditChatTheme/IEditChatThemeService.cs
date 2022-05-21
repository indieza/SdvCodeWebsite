// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditChatTheme
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.ViewModels;

    public interface IEditChatThemeService
    {
        ICollection<EditChatThemeViewModel> GetAllThemes();

        Task<GetEditChatThemeDataViewModel> GetThemeById(string themeId);

        Task<Tuple<bool, string>> EditChatTheme(EditChatThemeInputModel model);
    }
}