using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.Interfaces.IServices.INotification;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationSendingController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationSendingController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

       
    }
}
