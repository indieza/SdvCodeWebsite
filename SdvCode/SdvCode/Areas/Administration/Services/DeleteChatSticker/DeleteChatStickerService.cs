// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteChatSticker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteChatSticker.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class DeleteChatStickerService : IDeleteChatStickerService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DeleteChatStickerService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> DeleteChatSticker(DeleteChatStickerInputModel model)
        {
            var targetSticker = await this.db.Stickers.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetSticker != null)
            {
                string name = targetSticker.Name;
                ApplicationCloudinary.DeleteImage(
                        this.cloudinary,
                        string.Format(GlobalConstants.StickerName, model.Id),
                        GlobalConstants.StickersFolder);

                this.db.Stickers.Remove(targetSticker);
                await this.db.SaveChangesAsync();

                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyDeleteChatSticker, name.ToUpper()));
            }

            return Tuple.Create(false, ErrorMessages.StickerDoesNotExist);
        }

        public ICollection<DeleteChatStickerViewModel> GetAllStickers()
        {
            var result = new List<DeleteChatStickerViewModel>();
            var allStickers = this.db.Stickers.OrderBy(x => x.Name).ToList();

            foreach (var sticker in allStickers)
            {
                result.Add(new DeleteChatStickerViewModel
                {
                    Id = sticker.Id,
                    Name = sticker.Name,
                    Url = sticker.Url,
                });
            }

            return result;
        }

        public async Task<string> GetStickerUrl(string stickerId)
        {
            var targetStickerUrl = await this.db.Stickers
                .Where(x => x.Id == stickerId)
                .Select(x => x.Url)
                .FirstOrDefaultAsync();

            return targetStickerUrl;
        }
    }
}