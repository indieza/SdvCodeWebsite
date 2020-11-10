// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Administration.Services.AddEmoji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Server.IIS.Core;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Areas.Administration.ViewModels.AddEmojiViewModels.InputModels;
    using SdvCode.Areas.Administration.ViewModels.EditEmojiViewModels.InputModels;
    using SdvCode.Areas.PrivateChat.Models;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using Twilio.Rest.Authy.V1.Service.Entity.Factor;

    public class AddEmojiService : IAddEmojiService
    {
        private readonly ApplicationDbContext db;

        public AddEmojiService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Tuple<bool, string>> AddEmoji(AddEmojiInputModel model)
        {
            int emojiToUtf = char.ConvertToUtf32(model.Code, 0);

            if (this.db.Emojis.Any(x => x.Code == emojiToUtf))
            {
                return Tuple.Create(false, string.Format(ErrorMessages.EmojiAlreadyExist, model.Code));
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
                    Code = emojiToUtf,
                    EmojiType = model.EmojiType,
                    Name = model.Name,
                    Position = lastNumber + 1,
                };

                this.db.Emojis.Add(emoji);
                await this.db.SaveChangesAsync();
                return Tuple.Create(true, string.Format(SuccessMessages.SuccessfullyAddedEmoji, model.Code));
            }
        }
    }
}