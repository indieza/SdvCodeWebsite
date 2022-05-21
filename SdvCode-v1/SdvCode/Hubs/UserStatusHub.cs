// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Data;

    public class UserStatusHub : Hub
    {
        private static readonly List<string> OnlineUsers = new List<string>();
        private readonly ApplicationDbContext db;

        public UserStatusHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task IsUserOnline(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
            if (OnlineUsers.Contains(user.UserName))
            {
                await this.Clients.All.SendAsync("UserIsOnline", user.UserName);
            }
            else
            {
                await this.Clients.All.SendAsync("UserIsOffline", user.UserName);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var username = this.Context.User.Identity.Name;
            if (username != null)
            {
                OnlineUsers.Add(username);
                await this.Clients.All.SendAsync("UserIsOnline", username);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = this.Context.User.Identity.Name;
            if (username != null)
            {
                OnlineUsers.Remove(username);
                await this.Clients.All.SendAsync("UserIsOffline", username);
            }
        }
    }
}