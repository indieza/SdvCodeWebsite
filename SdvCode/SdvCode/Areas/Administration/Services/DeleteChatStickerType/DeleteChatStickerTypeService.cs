// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatStickerType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatStickerType.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class DeleteChatStickerTypeService : IDeleteChatStickerTypeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DeleteChatStickerTypeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> DeleteChatStickerType(DeleteChatStickerTypeInputModel model)
        {
            var targetStickerType = await this.db.StickerTypes.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetStickerType != null)
            {
                string name = targetStickerType.Name;
                int count = 0;

                var allStickers = this.db.Stickers.Where(x => x.StickerTypeId == targetStickerType.Id).ToList();

                foreach (var sticker in allStickers)
                {
                    ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.StickerName, sticker.Id),
                        GlobalConstants.StickersFolder);
                    count++;
                }

                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.StickerTypeName, targetStickerType.Id),
                    GlobalConstants.StickerTypeFolder);

                this.db.Stickers.RemoveRange(allStickers);
                this.db.StickerTypes.Remove(targetStickerType);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyDeleteChatStickerType, name, count));
            }

            return Tuple.Create(false, ErrorMessages.StickerTypeDoesNotExist);
        }

        public ICollection<DeleteChatStickerTypeViewModel> GetAllStickersTypes()
        {
            var result = new List<DeleteChatStickerTypeViewModel>();
            var allStickersTypes = this.db.StickerTypes.OrderBy(x => x.Name).ToList();

            foreach (var stickerType in allStickersTypes)
            {
                result.Add(new DeleteChatStickerTypeViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                });
            }

            return result;
        }

        public List<string> GetStickersUrls(string stickerTypeId)
        {
            var targetStickersUrls = this.db.Stickers.Where(x => x.StickerTypeId == stickerTypeId).Select(x => x.Url).ToList();

            return targetStickersUrls;
        }
    }
}