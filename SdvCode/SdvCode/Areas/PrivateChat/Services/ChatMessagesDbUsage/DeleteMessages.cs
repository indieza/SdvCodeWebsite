// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.PrivateChat.Services.ChatMessagesDbUsage
{
    using System.Linq;
    using SdvCode.Data;

    public class DeleteMessages : IDeleteMessages
    {
        private readonly ApplicationDbContext db;

        public DeleteMessages(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void DeleteAllChatMessages()
        {
            var target = this.db.ChatMessages.ToList();
            this.db.RemoveRange(target);
            this.db.SaveChanges();
        }
    }
}