using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Application.Interfaces.IMessageHub;
using PureLifeClinic.Infrastructure.SignalR.Hubs;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private IHubContext<NotificationHub, IMessageHub> _messageHub;
        public Test(IHubContext<NotificationHub, IMessageHub> messageHub)
        {
            _messageHub = messageHub;
        }
        [HttpPost]
        [Route("productoffers")]
        public string Get(string msg)
        {
            _messageHub.Clients.All.OnNotificationReceived(msg);
            return "Offers sent successfully to all users!";
        }

        [HttpGet("TestSendNotificationToOneUser")]
        public string TestSendNotificationToOneUser(string userId, string message)
        {
            _messageHub.Clients.User(userId).OnNewAppointmentReceived(message);

            return "Notification sent successfully to user!";
        }   
    }
}
