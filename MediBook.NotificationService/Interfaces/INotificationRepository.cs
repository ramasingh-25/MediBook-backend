using MediBook.NotificationService.Entities;

namespace MediBook.NotificationService.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification?> FindByNotificationId(string notificationId);
        Task<List<Notification>> FindByRecipientId(string recipientId);
        Task<List<Notification>> FindByRecipientIdAndIsRead(string recipientId, bool isRead);
        Task<int> CountByRecipientIdAndIsRead(string recipientId, bool isRead);
        Task<List<Notification>> FindByType(string type);
        Task<List<Notification>> FindByRelatedId(string relatedId);
        Task<Notification> CreateNotification(Notification notification);
        Task<Notification> UpdateNotification(Notification notification);
        Task<bool> DeleteNotification(string notificationId);
        Task<List<Notification>> GetAllNotifications();
    }
}
