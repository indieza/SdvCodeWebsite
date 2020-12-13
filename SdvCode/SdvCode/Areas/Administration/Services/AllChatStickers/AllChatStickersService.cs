// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AllChatStickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AllChatStickers.ViewModels;
    using SdvCode.Data;

    public class AllChatStickersService : IAllChatStickersService
    {
        private readonly ApplicationDbContext db;

        public AllChatStickersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AllChatStickersViewModel> GetAllChatStickers()
        {
            var result = new List<AllChatStickersViewModel>();

            var allStickersTypes = this.db.StickerTypes
                .OrderBy(x => x.Position)
                .ThenBy(x => x.Name)
                .ToList();

            foreach (var stickerType in allStickersTypes)
            {
                var targetType = new AllChatStickersViewModel
                {
                    Id = stickerType.Id,
                    Name = stickerType.Name,
                    Position = stickerType.Position,
                    Url = stickerType.Url,
                };

                var allStickers = this.db.Stickers
                    .Where(x => x.StickerTypeId == stickerType.Id)
                    .OrderBy(x => x.Position)
                    .ThenBy(x => x.Name)
                    .ToList();

                foreach (var sticker in allStickers)
                {
                    targetType.AllStickerst.Add(new AllChatStickersStickerViewModel
                    {
                        Id = sticker.Id,
                        Name = sticker.Name,
                        Position = sticker.Position,
                        Url = sticker.Url,
                    });
                }

                result.Add(targetType);
            }

            return result;
        }
    }
}