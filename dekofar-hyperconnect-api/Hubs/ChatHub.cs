using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Dekofar.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> Connections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                Connections[userId] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                Connections.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
