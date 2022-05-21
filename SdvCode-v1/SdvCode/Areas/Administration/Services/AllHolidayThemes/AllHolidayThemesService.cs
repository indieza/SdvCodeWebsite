// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AllHolidayThemes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml.ConditionalFormatting;
    using SdvCode.Areas.Administration.ViewModels.AllHolidayThemes.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AllHolidayThemesService : IAllHolidayThemesService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AllHolidayThemesService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> ChangeHolidayThemeStatus(string id, bool status)
        {
            var targetTheme = await this.db.HolidayThemes.FirstOrDefaultAsync(x => x.Id == id);

            if (targetTheme != null)
            {
                targetTheme.IsActive = status;
                this.db.HolidayThemes.Update(targetTheme);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(
                        SuccessMessages.SuccessfullyEditHolidayThemeStatus,
                        targetTheme.Name.ToUpper(),
                        status.ToString().ToUpper()));
            }

            return Tuple.Create(false, ErrorMessages.HolidayThemeDoesNotExist);
        }

        public async Task<Tuple<bool, string>> DeleteHolidayTheme(string id)
        {
            var targetTheme = await this.db.HolidayThemes.FirstOrDefaultAsync(x => x.Id == id);

            if (targetTheme != null)
            {
                var themeName = targetTheme.Name;

                var allThemeIcons = this.db.HolidayIcons.Where(x => x.HolidayThemeId == targetTheme.Id).ToList();

                foreach (var icon in allThemeIcons)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.HolidayIconName, icon.Id),
                        GlobalConstants.HolidayThemesFolder);
                }

                this.db.HolidayIcons.RemoveRange(allThemeIcons);
                this.db.HolidayThemes.Remove(targetTheme);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyDeleteHolidayTheme, themeName.ToUpper()));
            }

            return Tuple.Create(false, ErrorMessages.HolidayThemeDoesNotExist);
        }

        public ICollection<AllHolidayThemesViewModel> GetAllHolidayThemes()
        {
            var result = new List<AllHolidayThemesViewModel>();

            var allThemes = this.db.HolidayThemes.OrderBy(x => x.Name).ToList();

            foreach (var theme in allThemes)
            {
                result.Add(new AllHolidayThemesViewModel
                {
                    Id = theme.Id,
                    Name = theme.Name,
                    IsActive = theme.IsActive,
                    IconsUrls = this.db.HolidayIcons
                        .Where(x => x.HolidayThemeId == theme.Id)
                        .Select(x => x.Url)
                        .ToList(),
                });
            }

            return result;
        }
    }
}