// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class EditEmojiService : IEditEmojiService
    {
        private readonly ApplicationDbContext db;

        public EditEmojiService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Tuple<bool, string>> EditEmoji(EditEmojiInputModel model)
        {
            int emojiToUtf = char.ConvertToUtf32(model.Code, 0);
            if (this.db.Emojis.Any(x => x.Code == emojiToUtf && x.EmojiType == model.EmojiType))
            {
                return Tuple.Create(false, string.Format(ErrorMessages.EmojiAlreadyExist, model.Code));
            }

            var targetEmoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (targetEmoji != null)
            {
                targetEmoji.Name = model.Name;
                targetEmoji.Code = emojiToUtf;
                targetEmoji.EmojiType = model.EmojiType;
                this.db.Emojis.Update(targetEmoji);
                await this.db.SaveChangesAsync();
                return Tuple.Create(true, string.Format(SuccessMessages.SuccessfullyEditedEmoji, model.Code));
            }
            else
            {
                return Tuple.Create(false, ErrorMessages.EmojiDoesNotExist);
            }
        }

        public ICollection<EditEmojiViewModel> GetAllEmojis()
        {
            var emojis = this.db.Emojis.OrderBy(x => x.EmojiType).ThenBy(x => x.Position).ToList();
            var result = new List<EditEmojiViewModel>();
            foreach (var emoji in emojis)
            {
                result.Add(new EditEmojiViewModel
                {
                    Id = emoji.Id,
                    Name = $"{char.ConvertFromUtf32(emoji.Code)} - {emoji.Name}",
                });
            }

            return result;
        }

        public async Task<GetEditEmojiDataViewModel> GetEmojiById(string emojiId)
        {
            var emoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == emojiId);
            return new GetEditEmojiDataViewModel
            {
                Name = emoji.Name,
                EmojiType = emoji.EmojiType,
                Code = char.ConvertFromUtf32(emoji.Code),
            };
        }
    }
}