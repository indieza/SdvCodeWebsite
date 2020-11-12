// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatTheme
{
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddChatTheme.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddChatThemeService : IAddChatThemeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddChatThemeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> AddChatTheme(AddChatThemeInputModel model)
        {
            var targetTheme = await this.db.ChatThemes
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == model.Name.ToUpper());

            if (targetTheme != null)
            {
                return Tuple.Create(
                    false,
                    string.Format(ErrorMessages.ChatThemeAlreadyExist, model.Name.ToUpper()));
            }

            var imageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, model.Image, model.Name);
            targetTheme = new ChatTheme
            {
                Name = model.Name,
                Url = imageUrl,
            };

            this.db.ChatThemes.Add(targetTheme);
            await this.db.SaveChangesAsync();

            return Tuple.Create(
                true,
                string.Format(SuccessMessages.SuccessfullyAddedChatTheme, model.Name.ToUpper()));
        }
    }
}