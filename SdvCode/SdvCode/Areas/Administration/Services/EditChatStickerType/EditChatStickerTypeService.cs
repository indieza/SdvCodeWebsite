// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditChatStickerType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Hangfire.Storage.Monitoring;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditChatStickerType.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class EditChatStickerTypeService : IEditChatStickerTypeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public EditChatStickerTypeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> EditStickerType(EditChatStickerTypeInputModel model)
        {
            var targetStickerType = await this.db.StickerTypes.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetStickerType != null)
            {
                targetStickerType.Name = model.Name;
                if (model.Image != null)
                {
                    var imageUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        model.Image,
                        string.Format(GlobalConstants.StickerTypeName, model.Id),
                        GlobalConstants.StickerTypeFolder);

                    targetStickerType.Url = imageUrl;
                }

                this.db.StickerTypes.Update(targetStickerType);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(
                        SuccessMessages.SuccessfullyEditChatStickerType,
                        targetStickerType.Name.ToUpper()));
            }

            return Tuple.Create(false, ErrorMessages.StickerTypeDoesNotExist);
        }

        public ICollection<EditChatStickerTypeViewModel> GetAllChatStickerTypes()
        {
            var result = new List<EditChatStickerTypeViewModel>();
            var allStickerTypes = this.db.StickerTypes.OrderBy(x => x.Name).ToList();

            foreach (var stickerType in allStickerTypes)
            {
                result.Add(new EditChatStickerTypeViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                });
            }

            return result;
        }

        public async Task<GetEditChaStickerTypeDataViewModel> GetEmojiById(string stickerTypeId)
        {
            var targetStickerType = await this.db.StickerTypes.FirstOrDefaultAsync(x => x.Id == stickerTypeId);
            return new GetEditChaStickerTypeDataViewModel
            {
                Name = targetStickerType.Name,
                Url = targetStickerType.Url,
            };
        }
    }
}