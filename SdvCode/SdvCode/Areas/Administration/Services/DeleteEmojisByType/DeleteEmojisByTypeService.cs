// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.DeleteEmojisByType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using SdvCode.Areas.PrivateChat.Models.Enums;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class DeleteEmojisByTypeService : IDeleteEmojisByTypeService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public DeleteEmojisByTypeService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<string> DeleteEmojisByType(EmojiType emojiType)
        {
            var targetEmojis = this.db.Emojis.Where(x => x.EmojiType == emojiType).ToList();
            int count = 0;

            foreach (var emoji in targetEmojis)
            {
                var emojiSkins = this.db.EmojiSkins.Where(x => x.EmojiId == emoji.Id).ToList();
                foreach (var emojiSkin in emojiSkins)
                {
                    ApplicationCloudinary.DeleteImage(
                       this.cloudinary,
                       string.Format(GlobalConstants.EmojiSkin, emojiSkin.Id),
                       GlobalConstants.EmojisFolder);
                }

                ApplicationCloudinary.DeleteImage(
                    this.cloudinary,
                    string.Format(GlobalConstants.EmojiName, emoji.Id),
                    GlobalConstants.EmojisFolder);
                this.db.EmojiSkins.RemoveRange(emojiSkins);
                this.db.Emojis.Remove(emoji);
                this.db.SaveChanges();
                await this.db.SaveChangesAsync();
                count++;
            }

            return string.Format(
                SuccessMessages.SuccessfullyDeletedEmojisByType,
                count,
                emojiType.ToString().ToUpper());
        }
    }
}