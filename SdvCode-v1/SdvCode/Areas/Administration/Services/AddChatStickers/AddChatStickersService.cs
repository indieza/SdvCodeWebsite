// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddChatStickers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddChatStickers.ViewModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddChatStickersService : IAddChatStickersService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddChatStickersService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> AddChatStickers(AddChatStickersInputModel model)
        {
            var targetStickerType =
                await this.db.StickerTypes.FirstOrDefaultAsync(x => x.Id == model.StickerTypeId);

            if (targetStickerType != null)
            {
                var lastNumber = await this.db.Stickers
                     .Where(x => x.StickerTypeId == targetStickerType.Id)
                     .Select(x => x.Position)
                     .OrderByDescending(x => x)
                     .FirstOrDefaultAsync();

                var notAddedStickersCount = 0;
                var addedStickersCount = 0;

                foreach (var file in model.Images)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);

                    if (this.db.Stickers.Any(x => x.Name.ToUpper() == fileName.ToUpper() && x.StickerTypeId == targetStickerType.Id))
                    {
                        notAddedStickersCount++;
                    }
                    else
                    {
                        var sticker = new Sticker
                        {
                            Name = fileName.Length > 120 ? file.ToString().Substring(0, 120) : fileName,
                            Position = lastNumber + 1,
                            StickerTypeId = targetStickerType.Id,
                        };

                        var imageUrl = await ApplicationCloudinary.UploadImage(
                            this.cloudinary,
                            file,
                            string.Format(GlobalConstants.StickerName, sticker.Id),
                            GlobalConstants.StickersFolder);
                        sticker.Url = imageUrl;

                        lastNumber++;
                        addedStickersCount++;

                        this.db.Stickers.Add(sticker);
                        await this.db.SaveChangesAsync();
                    }
                }

                return Tuple.Create(
                    true,
                    string.Format(
                        SuccessMessages.SuccessfullyAddedStickers,
                        addedStickersCount,
                        notAddedStickersCount));
            }

            return Tuple.Create(false, ErrorMessages.StickerTypeDoesNotExist);
        }

        public ICollection<AddChatStickersViewModel> GetAllStickersTypes()
        {
            var result = new List<AddChatStickersViewModel>();
            var allStickersTypes = this.db.StickerTypes.OrderBy(x => x.Name).ToList();

            foreach (var stickerType in allStickersTypes)
            {
                result.Add(new AddChatStickersViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                });
            }

            return result;
        }
    }
}