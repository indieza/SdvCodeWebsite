// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class UserStatusHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var username = this.Context.User.Identity.Name;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var username = this.Context.User.Identity.Name;
            return base.OnDisconnectedAsync(exception);
        }
    }
}