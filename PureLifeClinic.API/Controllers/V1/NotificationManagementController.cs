using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.Interfaces.IServices.INotification;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationManagementController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationManagementController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //// Lấy danh sách thông báo của 1 user
        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetNotificationsByUserId(int userId)
        //{
        //    //var result = await _notificationService.GetNotificationsByUserIdAsync(userId);
        //    return Ok();
        //}

        //// Gửi thông báo mới
        //[HttpPost("send")]
        //public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto notification)
        //{
        //    var result = await _notificationService.SendNotificationAsync(notification);
        //    return Ok(result);
        //}

        //// Đánh dấu đã đọc thông báo
        //[HttpPut("{notificationId}/read")]
        //public async Task<IActionResult> MarkAsRead(int notificationId)
        //{
        //    await _notificationService.MarkAsReadAsync(notificationId);
        //    return NoContent();
        //}

        //// Đánh dấu tất cả đã đọc
        //[HttpPut("user/{userId}/read-all")]
        //public async Task<IActionResult> MarkAllAsRead(int userId)
        //{
        //    await _notificationService.MarkAllAsReadAsync(userId);
        //    return NoContent();
        //}
    }
}
