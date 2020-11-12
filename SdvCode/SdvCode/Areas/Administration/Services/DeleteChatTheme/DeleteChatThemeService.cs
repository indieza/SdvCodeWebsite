// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatTheme
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatTheme.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class DeleteChatThemeService : IDeleteChatThemeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DeleteChatThemeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> DeleteChatTheme(DeleteChatThemeInputModel model)
        {
            var targetTheme = await this.db.ChatThemes.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetTheme == null)
            {
                return Tuple.Create(
                    false,
                    string.Format(ErrorMessages.ChatThemeDoesNotAlreadyExist, model.Name.ToUpper()));
            }

            ApplicationCloudinary.DeleteImage(this.cloudinary, model.Name);
            this.db.ChatThemes.Remove(targetTheme);
            await this.db.SaveChangesAsync();

            return Tuple.Create(
                true,
                string.Format(SuccessMessages.SuccessfullyDeleteChatTheme, model.Name.ToUpper()));
        }

        public ICollection<DeleteChatThemeViewModel> GetAllChatThemes()
        {
            var result = new List<DeleteChatThemeViewModel>();

            foreach (var theme in this.db.ChatThemes.ToList())
            {
                result.Add(new DeleteChatThemeViewModel
                {
                    Id = theme.Id,
                    Name = theme.Name,
                });
            }

            return result;
        }

        public async Task<GetThemeDataViewModel> GetThemeById(int themeId)
        {
            var theme = await this.db.ChatThemes.FirstOrDefaultAsync(x => x.Id == themeId);

            return new GetThemeDataViewModel
            {
                Name = theme.Name,
                ImageUrl = theme.Url,
            };
        }
    }
}