// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.DeleteEmoji.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class DeleteEmojiService : IDeleteEmojiService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DeleteEmojiService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> DeleteEmoji(DeleteEmojiInputModel model)
        {
            var emoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == model.Id);
            var emojiType = emoji.EmojiType;

            if (emoji != null)
            {
                string emojiName = emoji.Name;

                var emojiSkins = this.db.EmojiSkins.Where(x => x.EmojiId == emoji.Id).ToList();
                foreach (var emojiSkin in emojiSkins)
                {
                    ApplicationCloudinary.DeleteImage(
                       this.cloudinary,
                       string.Format(GlobalConstants.EmojiSkin, emojiSkin.Id));
                }

                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.EmojiName, emoji.Id));
                this.db.EmojiSkins.RemoveRange(emojiSkins);
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

                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyDeleteEmoji, emojiName.ToUpper()));
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
                    Name = emoji.Name,
                });
            }

            return result;
        }

        public async Task<string> GetEmojiUrl(string emojiId)
        {
            var emoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == emojiId);
            return emoji.Url;
        }
    }
}