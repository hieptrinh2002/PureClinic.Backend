using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Interfaces.IMessageHub;
using PureLifeClinic.Core.MessageHub;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private IHubContext<NotificationHub, IMessageHub> messageHub;
        public Test(IHubContext<NotificationHub, IMessageHub> _messageHub)
        {
            messageHub = _messageHub;
        }
        [HttpPost]
        [Route("productoffers")]
        public string Get(string msg)
        {
            messageHub.Clients.All.ReceiveNotification(msg);
            return "Offers sent successfully to all users!";
        }

        [HttpGet("TestSendNotificationToOneUser")]
        public string TestSendNotificationToOneUser(string userId, string message)
        {
            messageHub.Clients.User(userId).ReceiveNewAppointment(message);

            return "Notification sent successfully to user!";
        }   
    }
}
