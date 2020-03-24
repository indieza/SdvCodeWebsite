// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using SdvCode.Data;

    public class PrivateChatHub : Hub
    {
        private readonly ApplicationDbContext db;

        public PrivateChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task SendMessage(string fromUsername, string toUsername, string message)
        {
            var toUser = this.db.Users.FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, message);
        }

        public async Task ReceiveMessage(string fromUsername, string message)
        {
            var fromUser = this.db.Users.FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, message);
        }
    }
}