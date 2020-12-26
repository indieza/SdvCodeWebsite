// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddHolidayTheme
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.Models.HolidayTheme;
    using SdvCode.Areas.Administration.ViewModels.AddHolidayTheme.InputModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddHolidayThemeService : IAddHolidayThemeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddHolidayThemeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> AddNewHolidayTheme(AddHolidayThemeInputModel model)
        {
            var targetTheme = await this.db.HolidayThemes
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == model.Name.ToUpper());

            if (targetTheme == null)
            {
                targetTheme = new HolidayTheme
                {
                    Name = model.Name,
                    IsActive = false,
                };

                foreach (var icon in model.Icons)
                {
                    var targetIcon = new HolidayIcon
                    {
                        Name = Path.GetFileNameWithoutExtension(icon.FileName),
                    };

                    var iconUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        icon,
                        string.Format(GlobalConstants.HolidayIconName, targetIcon.Id),
                        GlobalConstants.HolidayThemesFolder);

                    targetIcon.Url = iconUrl;
                    targetTheme.HolidayIcons.Add(targetIcon);
                }

                this.db.HolidayThemes.Add(targetTheme);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyAddedHolidayTheme, targetTheme.Name.ToUpper()));
            }

            return Tuple.Create(
                false,
                string.Format(ErrorMessages.HolidayThemeAlreadyExist, model.Name.ToUpper()));
        }
    }
}