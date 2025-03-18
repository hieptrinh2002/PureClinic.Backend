using PureLifeClinic.Core.Interfaces.IServices.INotification;
using System.Collections.Concurrent;

namespace PureLifeClinic.Infrastructure.SignalR
{
    public class HubConnectionService : IHubConnectionService
    {
        private static readonly ConcurrentDictionary<string, (string UserId, string Role)> _connections = new();

        public Task AddConnectionAsync(string connectionId, string userId, string role)
        {
            _connections[connectionId] = (userId, role);
            return Task.CompletedTask;
        }

        public Task RemoveConnectionAsync(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
            return Task.CompletedTask;
        }
    }
}
