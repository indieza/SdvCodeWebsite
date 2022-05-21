// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.EditEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmoji.ViewModels;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Services.Cloud;

    public class EditEmojiService : IEditEmojiService
    {
        private readonly ApplicationDbContext db;
        private readonly Cloudinary cloudinary;

        public EditEmojiService(ApplicationDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<Tuple<bool, string>> EditEmoji(EditEmojiInputModel model)
        {
            if (this.db.Emojis.Any(x => x.Name.ToUpper() == model.Name.ToUpper() && x.EmojiType == model.EmojiType))
            {
                return Tuple.Create(false, string.Format(ErrorMessages.EmojiAlreadyExist, model.Name.ToUpper()));
            }

            var targetEmoji = await this.db.Emojis.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (targetEmoji != null)
            {
                if (model.Image != null)
                {
                    var imageUrl = await ApplicationCloudinary.UploadImage(
                        this.cloudinary,
                        model.Image,
                        string.Format(GlobalConstants.EmojiName, model.Id),
                        GlobalConstants.EmojisFolder);
                    targetEmoji.Url = imageUrl;
                }

                targetEmoji.Name = model.Name;
                targetEmoji.EmojiType = model.EmojiType;
                this.db.Emojis.Update(targetEmoji);
                await this.db.SaveChangesAsync();
                return Tuple.Create(
                    true,
                    string.Format(SuccessMessages.SuccessfullyEditedEmoji, model.Name.ToUpper()));
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
                    Name = emoji.Name,
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
                Url = emoji.Url,
            };
        }
    }
}