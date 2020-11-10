// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmojiViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmojiViewModels.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;

    public class DeleteEmojiService : IDeleteEmojiService
    {
        private readonly ApplicationDbContext db;

        public DeleteEmojiService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Tuple<bool, string>> DeleteEmoji(DeleteEmojiInputModel model)
        {
            var emoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == model.Id);
            var emojiType = emoji.EmojiType;
            string emojiUtf = char.ConvertFromUtf32(emoji.Code);

            if (emoji != null)
            {
                this.db.Emojis.Remove(emoji);
                this.db.SaveChanges();

                var targetToUpdate = this.db.Emojis
                    .Where(x => x.EmojiType == emojiType)
                    .OrderBy(x => x.Position)
                    .ToList();

                for (int i = 0; i < targetToUpdate.Count(); i++)
                {
                    targetToUpdate[i].Position = i + 1;
                }

                this.db.Emojis.UpdateRange(targetToUpdate);
                await this.db.SaveChangesAsync();

                return Tuple.Create(true, emojiUtf);
            }
            else
            {
                return Tuple.Create(false, ErrorMessages.EmojiDoesNotExist);
            }
        }

        public ICollection<DeleteEmojiViewModel> GetAllEmojis()
        {
            var emojis = this.db.Emojis.OrderBy(x => x.EmojiType).ThenBy(x => x.Position).ToList();
            var result = new List<DeleteEmojiViewModel>();
            foreach (var emoji in emojis)
            {
                result.Add(new DeleteEmojiViewModel
                {
                    Id = emoji.Id,
                    Name = $"{char.ConvertFromUtf32(emoji.Code)} - {emoji.Name}",
                });
            }

            return result;
        }
    }
}