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

        public async Task SendMessage(string user, string message)
        {
            var id = this.db.Users.FirstOrDefault(x => x.UserName == user).Id;
            await this.Clients.User(id).SendAsync("ReceiveMessage", user, message);
        }
    }
}