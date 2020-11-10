// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditEmojiPosition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPositionViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiPositionViewModels.ViewModels;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Data;
    using Twilio.Rest.Api.V2010.Account.Usage.Record;

    public class EditEmojiPositionService : IEditEmojiPositionService
    {
        private readonly ApplicationDbContext db;

        public EditEmojiPositionService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> EditEmojisPosition(EditEmojiPositionInputModel[] allEmojis)
        {
            var count = 0;
            foreach (var emoji in allEmojis)
            {
                var targetEmoji = await this.db.Emojis
                    .FirstOrDefaultAsync(x => x.Id == emoji.Id && x.Name == emoji.Name);
                if (targetEmoji.Position != emoji.Position)
                {
                    count++;
                    targetEmoji.Position = emoji.Position;
                    this.db.Emojis.Update(targetEmoji);
                }
            }

            await this.db.SaveChangesAsync();
            return count;
        }

        public ICollection<EditEmojiPositionViewModel> GetAllEmojisByType(EmojiType emojiType)
        {
            var emojis = this.db.Emojis.Where(x => x.EmojiType == emojiType).OrderBy(x => x.Position).ToList();
            var result = new List<EditEmojiPositionViewModel>();

            foreach (var emoji in emojis)
            {
                result.Add(new EditEmojiPositionViewModel
                {
                    Id = emoji.Id,
                    Name = emoji.Name,
                    Code = char.ConvertFromUtf32(emoji.Code),
                    Position = emoji.Position,
                    EmojiType = emoji.EmojiType,
                });
            }

            return result;
        }
    }
}