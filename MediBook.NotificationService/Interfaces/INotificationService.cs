using MediBook.NotificationService.DTOs;

namespace MediBook.NotificationService.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationResponse> Send(SendNotificationRequest request);
        Task<List<NotificationResponse>> SendBulk(SendBulkNotificationRequest request);
        Task<NotificationResponse?> MarkAsRead(string notificationId);
        Task<bool> MarkAllRead(string recipientId);
        Task<List<NotificationResponse>> GetByRecipient(string recipientId);
        Task<int> GetUnreadCount(string recipientId);
        Task<bool> DeleteNotification(string notificationId);
        Task<List<NotificationResponse>> GetAllNotifications();
    }
}
