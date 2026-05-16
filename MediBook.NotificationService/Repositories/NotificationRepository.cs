using MediBook.NotificationService.Data;
using MediBook.NotificationService.Entities;
using MediBook.NotificationService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.NotificationService.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> FindByNotificationId(string notificationId)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<List<Notification>> FindByRecipientId(string recipientId)
        {
            return await _context.Notifications
                .Where(n => n.RecipientId == recipientId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> FindByRecipientIdAndIsRead(string recipientId, bool isRead)
        {
            return await _context.Notifications
                .Where(n => n.RecipientId == recipientId && n.IsRead == isRead)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<int> CountByRecipientIdAndIsRead(string recipientId, bool isRead)
        {
            return await _context.Notifications
                .Where(n => n.RecipientId == recipientId && n.IsRead == isRead)
                .CountAsync();
        }

        public async Task<List<Notification>> FindByType(string type)
        {
            return await _context.Notifications
                .Where(n => n.Type == type)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> FindByRelatedId(string relatedId)
        {
            return await _context.Notifications
                .Where(n => n.RelatedId == relatedId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<Notification> CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotification(string notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            return await _context.Notifications
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }
    }
}
