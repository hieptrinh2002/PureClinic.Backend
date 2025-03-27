namespace PureLifeClinic.Application.Interfaces.IServices.INotification
{
    //public interface INotificationService
    //{
    //    Task SendNotificationAsync(string message);
    //    Task NotifyEmployeeAsync(string message);
    //    Task ReceiveNewAppointmentAsync(string userId, string message);
    //}

    public interface INotificationService
    {
        Task SendToAllAsync(string eventName, object message);
        Task SendToUserAsync(string userId, string eventName, object message);
        Task SendToGroupAsync(string groupName, string eventName, object message);
        Task SendToUsersAsync(IEnumerable<string> userIds, string eventName, object message);
        Task SendToGroupsAsync(IEnumerable<string> groupNames, string eventName, object message);
    }
}