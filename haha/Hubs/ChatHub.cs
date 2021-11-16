using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace haha.Hubs
{

    public class ChatHub:Hub
    {
        public string id = string.Empty;
        public async Task AddToGroup(string groupName)
        {
             id = Context.ConnectionId;
            await Groups.AddToGroupAsync(id, groupName);
            
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task Send(string name, string message)
        {
            
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendPrivateMessage(string userid, string message)
        {
            await Clients.User(userid).SendAsync(message);
        }
    }
}
