using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Core.Interfaces.IMessageHub;
using PureLifeClinic.Core.MessageHub;
using Xunit.Sdk;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHub> messageHub;
        public Test(IHubContext<MessageHub, IMessageHub> _messageHub)
        {
            messageHub = _messageHub;
        }
        [HttpPost]
        [Route("productoffers")]
        public string Get()
        {
            List<string> offers = new List<string>();
            offers.Add("20% Off on IPhone 12");
            offers.Add("15% Off on HP Pavillion");
            offers.Add("25% Off on Samsung Smart TV");
            messageHub.Clients.All.SendNotification(offers);
            return "Offers sent successfully to all users!";
        }
    }
}
