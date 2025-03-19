using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Common.Constants;
using System.Collections.Concurrent;

namespace PureLifeClinic.Infrastructure.SignalR.Hubs
{
    public class UserAuthHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            string newConnectionId = Context.ConnectionId;

            if (_userConnections.TryGetValue(userId, out string oldConnectionId))
            {
                await Clients.Client(oldConnectionId)
                        .SendAsync(EventConstants.OnForceLogout, "You have been logged out due to logging in from another device."); 
            }

            _userConnections[userId] = newConnectionId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.UserIdentifier;
            string connectionId = Context.ConnectionId;

            _userConnections.TryRemove(userId, out _);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
