using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Interfaces.IMessageHub;

namespace PureLifeClinic.Core.MessageHub
{
    [Authorize]
    public class NotificationHub : Hub<IMessageHub>
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Employee"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
            }
            else
            {
                string userId = Context.UserIdentifier;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            } 
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User.IsInRole("Employee"))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Employee");
            }
            else
            {
                string userId = Context.UserIdentifier;
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
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
    