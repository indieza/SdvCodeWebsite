// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AllHolidayThemes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AllHolidayThemes.ViewModels;

    public interface IAllHolidayThemesService
    {
        ICollection<AllHolidayThemesViewModel> GetAllHolidayThemes();

        Task<Tuple<bool, string>> ChangeHolidayThemeStatus(string id, bool status);
        Task<Tuple<bool, string>> DeleteHolidayTheme(string id);
    }
}