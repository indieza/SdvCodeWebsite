// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmojis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddEmojisService : IAddEmojisService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddEmojisService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<string> AddEmojis(AddEmojisInputModel model)
        {
            var addedEmojisCount = 0;
            var notAddedEmojisCount = 0;

            var lastNumber = await this.db.Emojis
                 .Where(x => x.EmojiType == model.EmojiType)
                 .Select(x => x.Position)
                 .OrderByDescending(x => x)
                 .FirstOrDefaultAsync();

            foreach (var file in model.Images)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);

                if (this.db.Emojis.Any(x => x.Name.ToUpper() == fileName.ToUpper() && x.EmojiType == model.EmojiType))
                {
                    notAddedEmojisCount++;
                }
                else
                {
                    var emoji = new Emoji
                    {
                        Name = fileName.Length > 120 ? file.ToString().Substring(0, 120) : fileName,
                        Position = lastNumber + 1,
                        EmojiType = model.EmojiType,
                    };

                    var emojiUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        file,
                        string.Format(GlobalConstants.EmojiName, emoji.Id),
                        GlobalConstants.EmojisFolder);
                    emoji.Url = emojiUrl;

                    lastNumber++;
                    addedEmojisCount++;

                    this.db.Emojis.Add(emoji);
                    await this.db.SaveChangesAsync();
                }
            }

            return string.Format(SuccessMessages.SuccessfullyAddedEmojis, addedEmojisCount, notAddedEmojisCount);
        }
    }
}