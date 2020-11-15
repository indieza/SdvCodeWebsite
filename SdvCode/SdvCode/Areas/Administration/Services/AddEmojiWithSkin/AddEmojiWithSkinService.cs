// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmojiWithSkin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddEmojis.InputModels;
    using SdvCode.Areas.Administration.ViewModels.AddEmojiWithSkin.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class AddEmojiWithSkinService : IAddEmojiWithSkinService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public AddEmojiWithSkinService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<string> AddEmojiWithSkin(AddEmojiWithSkinInputModel model)
        {
            var targetEmoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Name.ToUpper() == model.Name.ToUpper() && x.EmojiType == model.EmojiType);
            var addedSkins = 0;

            if (targetEmoji == null)
            {
                var lastEmojiNumber = await this.db.Emojis
                   .Where(x => x.EmojiType == model.EmojiType)
                   .Select(x => x.Position)
                   .OrderByDescending(x => x)
                   .FirstOrDefaultAsync();

                targetEmoji = new Emoji
                {
                    Name = model.Name,
                    EmojiType = model.EmojiType,
                    Position = lastEmojiNumber + 1,
                };

                var imgeUrl = await ApplicationCloudinary.UploadImage(
                    this.cloudinary,
                    model.Image,
                    string.Format(GlobalConstants.EmojiName, targetEmoji.Id));
                targetEmoji.Url = imgeUrl;

                var lastEmojiSkinNumber = await this.db.EmojiSkins
                   .Where(x => x.EmojiId == targetEmoji.Id)
                   .Select(x => x.Position)
                   .OrderByDescending(x => x)
                   .FirstOrDefaultAsync();
                foreach (var skinFile in model.ImageSkins)
                {
                    var fileName = Path.GetFileNameWithoutExtension(skinFile.FileName);
                    var emojiSkin = new EmojiSkin
                    {
                        EmojiId = targetEmoji.Id,
                        Name = fileName,
                        Position = lastEmojiSkinNumber + 1,
                    };

                    var skinUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        skinFile,
                        string.Format(GlobalConstants.EmojiSkin, emojiSkin.Id));
                    emojiSkin.Url = skinUrl;

                    this.db.Emojis.Add(targetEmoji);
                    this.db.EmojiSkins.Add(emojiSkin);
                    lastEmojiSkinNumber++;
                    addedSkins++;
                }

                await this.db.SaveChangesAsync();
                return string.Format(
                    SuccessMessages.SuccessfullyAddedEmojiWithSkins,
                    model.Name.ToUpper(),
                    addedSkins);
            }
            else
            {
                var targetSkins = this.db.EmojiSkins.Where(x => x.EmojiId == targetEmoji.Id).ToList();

                var lastEmojiSkinNumber = await this.db.EmojiSkins
                    .Where(x => x.EmojiId == targetEmoji.Id)
                    .Select(x => x.Position)
                    .OrderByDescending(x => x)
                    .FirstOrDefaultAsync();
                foreach (var skinFile in model.ImageSkins)
                {
                    var fileName = Path.GetFileNameWithoutExtension(skinFile.FileName);
                    if (!targetSkins.Any(x => x.Name.ToUpper() == fileName.ToUpper()))
                    {
                        var emojiSkin = new EmojiSkin
                        {
                            EmojiId = targetEmoji.Id,
                            Name = fileName,
                            Position = lastEmojiSkinNumber + 1,
                        };

                        var skinUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        skinFile,
                        string.Format(GlobalConstants.EmojiSkin, emojiSkin.Id));
                        emojiSkin.Url = skinUrl;

                        targetSkins.Add(emojiSkin);
                        lastEmojiSkinNumber++;
                        addedSkins++;
                        this.db.EmojiSkins.Add(emojiSkin);
                    }
                }

                await this.db.SaveChangesAsync();
                return string.Format(
                    SuccessMessages.SuccessfullyAddedEmojiWithSkins,
                    model.Name.ToUpper(),
                    addedSkins);
            }
        }
    }
}