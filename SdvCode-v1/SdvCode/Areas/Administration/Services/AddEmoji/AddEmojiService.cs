// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Server.IIS.Core;
    using Microsoft.EntityFrameworkCore;

    using SdvCode.Areas.Administration.ViewModels.AddEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddEmojiService : IAddEmojiService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddEmojiService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> AddEmoji(AddEmojiInputModel model)
        {
            if (this.db.Emojis.Any(x => x.Name.ToUpper() == model.Name.ToUpper() && x.EmojiType == model.EmojiType))
            {
                return Tuple.Create(false, string.Format(ErrorMessages.EmojiAlreadyExist, model.Name.ToUpper()));
            }
            else
            {
                var lastNumber = await this.db.Emojis
                .Where(x => x.EmojiType == model.EmojiType)
                .Select(x => x.Position)
                .OrderByDescending(x => x)
                .FirstOrDefaultAsync();
                var emoji = new Emoji
                {
                    EmojiType = model.EmojiType,
                    Name = model.Name,
                    Position = lastNumber + 1,
                };

                var imageUrl = await ApplicationCloudinary.UploadImage(
                    this.cloudinary,
                    model.Image,
                    string.Format(GlobalConstants.EmojiName, emoji.Id),
                    GlobalConstants.EmojisFolder);
                emoji.Url = imageUrl;

                this.db.Emojis.Add(emoji);
                await this.db.SaveChangesAsync();
                return Tuple.Create(true, string.Format(SuccessMessages.SuccessfullyAddedEmoji, emoji.Name));
            }
        }
    }
}