namespace PureLifeClinic.Core.Interfaces.IServices.INotification
{
    public interface IHubConnectionService
    {
        Task AddConnectionAsync(string connectionId, string userId, string role);
        Task RemoveConnectionAsync(string connectionId);
    }
}
