// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatStickerType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickerType.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddChatStickerTypeService : IAddChatStickerTypeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddChatStickerTypeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> AddNewStickerType(AddChatStickerTypeInputModel model)
        {
            if (this.db.StickerTypes.Any(x => x.Name.ToUpper() == model.Name.ToUpper()))
            {
                return Tuple.Create(
                    false,
                    string.Format(ErrorMessages.StickerTypeAlreadyExist, model.Name.ToUpper()));
            }

            var stickerType = new StickerType
            {
                Name = model.Name,
                Position = await this.db.StickerTypes
                    .Select(x => x.Position)
                    .OrderByDescending(x => x)
                    .FirstOrDefaultAsync() + 1,
            };

            var imageUrl = await ApplicationCloudinary.UploadImage(
                this.cloudinary,
                model.Image,
                string.Format(GlobalConstants.StickerTypeName, stickerType.Id),
                GlobalConstants.StickerTypeFolder);

            stickerType.Url = imageUrl;

            this.db.StickerTypes.Add(stickerType);
            await this.db.SaveChangesAsync();

            return Tuple.Create(
                true,
                string.Format(SuccessMessages.SuccessfullyAddedStickerType, stickerType.Name.ToUpper()));
        }
    }
}