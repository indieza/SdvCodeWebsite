// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.CollectStickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.PrivateChat.ViewModels.CollectStickers.ViewModels;
    using SdvCode.Data;
    using SdvCode.Models.User;

    public class CollectStickersService : ICollectStickersService
    {
        private readonly ApplicationDbContext db;

        public CollectStickersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<CollectStickersStickerTypeViewModel> GetAllStickers(ApplicationUser currentUser)
        {
            var result = new List<CollectStickersStickerTypeViewModel>();

            var allStickerTypes = this.db.StickerTypes.OrderBy(x => x.Name).ThenBy(x => x.Position).ToList();

            foreach (var stickerType in allStickerTypes)
            {
                var targetStickerType = new CollectStickersStickerTypeViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                    Url = stickerType.Url,
                };

                var allStickers = this.db.Stickers
                    .Where(x => x.StickerTypeId == stickerType.Id)
                    .OrderBy(x => x.Position)
                    .ToList();

                foreach (var sticker in allStickers)
                {
                    targetStickerType.AllStickers.Add(new CollectStickersStickerViewModel
                    {
                        Id = sticker.Id,
                        Name = sticker.Name,
                        Url = sticker.Url,
                    });
                }

                result.Add(targetStickerType);
            }

            return result;
        }
    }
}