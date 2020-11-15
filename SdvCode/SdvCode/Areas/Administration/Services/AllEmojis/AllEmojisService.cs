// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AllEmojis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.Administration.ViewModels.AllEmojis.ViewModels;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Data;

    public class AllEmojisService : IAllEmojisService
    {
        private readonly ApplicationDbContext db;

        public AllEmojisService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Dictionary<EmojiType, ICollection<EmojiViewModel>> GetAllEmojis()
        {
            var result = new Dictionary<EmojiType, ICollection<EmojiViewModel>>();

            foreach (var emojiType in Enum.GetValues(typeof(EmojiType)))
            {
                result.Add((EmojiType)emojiType, new List<EmojiViewModel>());
                var emojis = this.db.Emojis
                    .Where(x => x.EmojiType == (EmojiType)emojiType)
                    .OrderBy(x => x.Position)
                    .ToList();

                foreach (var emoji in emojis)
                {
                    result[(EmojiType)emojiType].Add(new EmojiViewModel
                    {
                        Id = emoji.Id,
                        Name = emoji.Name,
                        Position = emoji.Position,
                        Url = emoji.Url,
                        EmojiType = emoji.EmojiType,
                        SkinsUrls = this.db.EmojiSkins.Where(x => x.EmojiId == emoji.Id).OrderBy(x => x.Position).Select(x => x.Url).ToList(),
                    });
                }
            }

            return result;
        }
    }
}