// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatTheme
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.ViewModels;

    public interface IDeleteChatThemeService
    {
        ICollection<DeleteChatThemeViewModel> GetAllChatThemes();

        Task<GetThemeDataViewModel> GetThemeById(int themeId);

        Task<Tuple<bool, string>> DeleteChatTheme(DeleteChatThemeInputModel model);
    }
}