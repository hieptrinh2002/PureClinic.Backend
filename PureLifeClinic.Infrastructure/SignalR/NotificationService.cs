using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Application.Interfaces.IServices.INotification;
using PureLifeClinic.Infrastructure.SignalR.Hubs;

namespace PureLifeClinic.Infrastructure.SignalR
{
    //public class NotificationService : INotificationService
    //{
    //    private readonly IHubContext<NotificationHub, IMessageHub> _hubContext;

    //    public NotificationService(IHubContext<NotificationHub, IMessageHub> hubContext)
    //    {
    //        _hubContext = hubContext;
    //    }

    //    public async Task SendNotificationToAllUserAsync(string message)
    //    {
    //        await _hubContext.Clients.All.OnNotificationReceived(message);
    //    }

    //    public async Task SendNotificationToUserAsync(string userId, string message)
    //    {
    //        await _hubContext.Clients.User(userId).OnNotificationReceived(message);
    //    }

    //    public async Task NotifyEmployeeAsync(string message)
    //    {
    //        await _hubContext.Clients.Group("Employee").OnNewAppointmentReceived(message);
    //    }

    //    public async Task NotifyReceiveNewAppointmentGroupAsync(string groupName, string message)
    //    {
    //        await _hubContext.Clients.Group(groupName).OnNewAppointmentReceived(message);
    //    }
    //}

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToAllAsync(string eventName, object message)
        {
            await _hubContext.Clients.All.SendAsync(eventName, message);
        }

        public async Task SendToUserAsync(string userId, string eventName, object message)
        {
            await _hubContext.Clients.User(userId).SendAsync(eventName, message);
        }

        public async Task SendToGroupAsync(string groupName, string eventName, object message)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(eventName, message);
        }

        public async Task SendToUsersAsync(IEnumerable<string> userIds, string eventName, object message)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId).SendAsync(eventName, message);
            }
        }

        public async Task SendToGroupsAsync(IEnumerable<string> groupNames, string eventName, object message)
        {
            foreach (var groupName in groupNames)
            {
                await _hubContext.Clients.Group(groupName).SendAsync(eventName, message);
            }
        }
    }
}
