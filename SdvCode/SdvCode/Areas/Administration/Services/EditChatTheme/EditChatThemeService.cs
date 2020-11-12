// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditChatTheme
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatTheme.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class EditChatThemeService : IEditChatThemeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public EditChatThemeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> EditChatTheme(EditChatThemeInputModel model)
        {
            var targetTheme = await this.db.ChatThemes.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetTheme == null)
            {
                return Tuple.Create(
                    false, ErrorMessages.ChatThemeDoesNotAlreadyExist);
            }

            if (this.db.ChatThemes.Any(x => x.Name.ToUpper() == model.Name.ToUpper()) && model.Image == null)
            {
                return Tuple.Create(
                    false,
                    string.Format(ErrorMessages.ChatThemeAlreadyExist, model.Name.ToUpper()));
            }

            if (model.Image != null)
            {
                string imageUrl =
                    await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        model.Image,
                        string.Format(GlobalConstants.ChatThemeName, targetTheme.Id));
                targetTheme.Url = imageUrl;
            }

            targetTheme.Name = model.Name;
            this.db.ChatThemes.Update(targetTheme);
            await this.db.SaveChangesAsync();

            return Tuple.Create(
                true,
                string.Format(SuccessMessages.SuccessfullyEditChatTheme, model.Name.ToUpper()));
        }

        public ICollection<EditChatThemeViewModel> GetAllThemes()
        {
            var result = new List<EditChatThemeViewModel>();

            foreach (var theme in this.db.ChatThemes.ToList())
            {
                result.Add(new EditChatThemeViewModel
                {
                    Id = theme.Id,
                    Name = theme.Name,
                    OldName = theme.Name,
                });
            }

            return result;
        }

        public async Task<GetEditChatThemeDataViewModel> GetThemeById(string themeId)
        {
            var theme = await this.db.ChatThemes.FirstOrDefaultAsync(x => x.Id == themeId);

            return new GetEditChatThemeDataViewModel
            {
                Name = theme.Name,
                ImageUrl = theme.Url,
            };
        }
    }
}