using Microsoft.AspNetCore.Mvc;
using MediBook.NotificationService.DTOs;
using MediBook.NotificationService.Interfaces;

namespace MediBook.NotificationService.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<NotificationResponse>> Send([FromBody] SendNotificationRequest request)
        {
            var response = await _notificationService.Send(request);
            return Ok(response);
        }

        [HttpPost("sendBulk")]
        public async Task<ActionResult<List<NotificationResponse>>> SendBulk([FromBody] SendBulkNotificationRequest request)
        {
            var responses = await _notificationService.SendBulk(request);
            return Ok(responses);
        }

        [HttpGet("recipient/{recipientId}")]
        public async Task<ActionResult<List<NotificationResponse>>> GetByRecipient(string recipientId)
        {
            var notifications = await _notificationService.GetByRecipient(recipientId);
            return Ok(notifications);
        }

        [HttpPut("{notificationId}/markAsRead")]
        public async Task<ActionResult<NotificationResponse>> MarkAsRead(string notificationId)
        {
            var notification = await _notificationService.MarkAsRead(notificationId);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }
            return Ok(notification);
        }

        [HttpPut("recipient/{recipientId}/markAllRead")]
        public async Task<ActionResult<bool>> MarkAllRead(string recipientId)
        {
            var result = await _notificationService.MarkAllRead(recipientId);
            return Ok(result);
        }

        [HttpGet("recipient/{recipientId}/unreadCount")]
        public async Task<ActionResult<int>> GetUnreadCount(string recipientId)
        {
            var count = await _notificationService.GetUnreadCount(recipientId);
            return Ok(count);
        }

        [HttpDelete("{notificationId}")]
        public async Task<ActionResult<bool>> DeleteNotification(string notificationId)
        {
            var result = await _notificationService.DeleteNotification(notificationId);
            if (!result)
            {
                return NotFound(new { message = "Notification not found." });
            }
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<NotificationResponse>>> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotifications();
            return Ok(notifications);
        }
    }
}
