using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Interfaces.IMessageHub;

namespace PureLifeClinic.Core.MessageHub
{
    public class MessageHub : Hub<IMessageHub>
    {
        public async Task SendNotification(List<string> message)
        {
            await Clients.All.SendNotification(message);
        }
    }
}
