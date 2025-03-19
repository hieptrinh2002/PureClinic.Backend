using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IServices.INotification
{
    public interface IHubConnectionService
    {
        Task AddConnectionAsync(string userId, string connectionId, string device, string ip);
        Task RemoveConnectionAsync(string connectionId);
        Task<List<UserConnection>> GetUserConnectionsAsync(string userId);
    }
}
