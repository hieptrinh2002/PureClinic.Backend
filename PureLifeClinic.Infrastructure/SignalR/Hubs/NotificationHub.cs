using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Interfaces.IMessageHub;
using PureLifeClinic.Core.Interfaces.IServices.INotification;

namespace PureLifeClinic.Infrastructure.SignalR.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<IMessageHub>
    {
        //public override async Task OnConnectedAsync()
        //{
        //    if (Context.User.IsInRole("Employee"))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
        //    }
        //    else
        //    {
        //        string userId = Context.UserIdentifier;
        //        await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        //    }
        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    if (Context.User.IsInRole("Employee"))
        //    {
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Employee");
        //    }
        //    else
        //    {
        //        string userId = Context.UserIdentifier;
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}

        private readonly IHubConnectionService _connectionService;

        public NotificationHub(IHubConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            string role = Context.User.IsInRole("Employee") ? "Employee" : "User";

            await _connectionService.AddConnectionAsync(Context.ConnectionId, userId, role);
            await Groups.AddToGroupAsync(Context.ConnectionId, role == "Employee" ? "Employee" : $"User_{userId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _connectionService.RemoveConnectionAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string message)
        {
            await Clients.All.OnNotificationReceived(message);
        }

        public async Task NotifyEmployee(string userId, string message)
        {
            await Clients.Group("Employee").OnNewAppointmentReceived(message);
        }

        public async Task ReceiveNewAppointment(string userId, string message)
        {
            //await Clients.User(userId).ReceiveNewAppointment(userId, message);
            await Clients.Group("Employee").OnNewAppointmentReceived(message);
        }
    }
}
