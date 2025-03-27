using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Application.Interfaces.IServices.INotification;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.SignalR
{
    public class HubConnectionService : IHubConnectionService
    {
        private readonly ApplicationDbContext _dbContext;

        public HubConnectionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddConnectionAsync(string userId, string connectionId, string device, string ip)
        {
            var connection = new UserConnection
            {
                UserId = int.Parse(userId),
                ConnectionId = connectionId,
                Device = device,
                IpAddress = ip,
                IsOnline = true
            };

            _dbContext.UserConnections.Add(connection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveConnectionAsync(string connectionId)
        {
            var connection = await _dbContext.UserConnections.FirstOrDefaultAsync(c => c.ConnectionId == connectionId);

            if (connection != null)
            {
                _dbContext.UserConnections.Remove(connection);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserConnection>> GetUserConnectionsAsync(string userId)
        {
            return await _dbContext.UserConnections
                .Where(c => c.UserId == int.Parse(userId) && c.IsOnline)
                .ToListAsync();
        }
    }
}
